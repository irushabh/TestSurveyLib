using Microsoft.AspNetCore.Components;
using SurveyBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyBuilder.Components
{
    public partial class SurveyRunner
    {
        [Parameter] public string SurveyTitle { get; set; } = "Survey";
        [Parameter] public List<SurveyQuestion> Questions { get; set; } = new();
        [Parameter] public EventCallback<List<SurveyResponse>> OnSubmit { get; set; }

        private Dictionary<Guid, SurveyResponse> _responses = new();
        private bool _submitted;
        private string? _validationError;

        protected override void OnParametersSet()
        {
            _responses = Questions.ToDictionary(
                q => q.Id,
                q => new SurveyResponse { QuestionId = q.Id });
        }

        private void SetText(Guid id, ChangeEventArgs e) =>
            _responses[id].TextAnswer = e.Value?.ToString();

        private void SetSingle(Guid id, ChangeEventArgs e)
        {
            _responses[id].SelectedOptions.Clear();
            _responses[id].SelectedOptions.Add(e.Value?.ToString() ?? "");
        }

        private void ToggleCheckbox(Guid id, string option, ChangeEventArgs e)
        {
            var list = _responses[id].SelectedOptions;
            if (e.Value is true) { if (!list.Contains(option)) list.Add(option); }
            else list.Remove(option);
        }

        private void SetRating(Guid id, int star) =>
            _responses[id].RatingAnswer = star;

        private void SetYesNo(Guid id, bool val) =>
            _responses[id].YesNoAnswer = val;

        private async Task Submit()
        {
            _validationError = null;
            foreach (var q in Questions.Where(q => q.IsRequired))
            {
                var r = _responses[q.Id];
                bool answered = q.Type switch
                {
                    QuestionType.Text => !string.IsNullOrWhiteSpace(r.TextAnswer),
                    QuestionType.MultipleChoice => r.SelectedOptions.Any(),
                    QuestionType.Checkbox => r.SelectedOptions.Any(),
                    QuestionType.Rating => r.RatingAnswer.HasValue,
                    QuestionType.YesNo => r.YesNoAnswer.HasValue,
                    QuestionType.Dropdown => r.SelectedOptions.Any(),
                    _ => true
                };
                if (!answered)
                {
                    _validationError = $"Please answer all required questions (marked with *).";
                    return;
                }
            }
            _submitted = true;
            await OnSubmit.InvokeAsync(_responses.Values.ToList());
        }

        private bool IsValidAnswer(SurveyQuestion? q)
        {
            bool Result = true;
            if (!string.IsNullOrEmpty(_validationError))
            {
                if (q.IsRequired)
                {
                    var r = _responses[q.Id];
                    Result = q.Type switch
                    {
                        QuestionType.Text => !string.IsNullOrEmpty(r.TextAnswer),
                        QuestionType.MultipleChoice => r.SelectedOptions.Any(),
                        QuestionType.Checkbox => r.SelectedOptions.Any(),
                        QuestionType.Rating => r.RatingAnswer.HasValue,
                        QuestionType.YesNo => r.YesNoAnswer.HasValue,
                        QuestionType.Dropdown => r.SelectedOptions.Any(),
                        _ => true
                    };
                }
                else
                    Result = true;
            }
            return Result;
        }
    }
}

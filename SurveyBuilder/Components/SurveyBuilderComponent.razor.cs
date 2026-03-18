using Microsoft.AspNetCore.Components;
using SurveyBuilder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SurveyBuilder.Components
{
    public partial class SurveyBuilderComponent
    {
        [Parameter] public string Title { get; set; } = "My Survey";
        [Parameter] public EventCallback<List<SurveyQuestion>> OnSurveyReady { get; set; }

        private List<SurveyQuestion> _questions = new();
        private string _surveyTitle = "My Survey";

        protected override void OnParametersSet() => _surveyTitle = Title;

        private void AddQuestion() =>
            _questions.Add(new SurveyQuestion { Text = $"Question {_questions.Count + 1}" });

        private void RemoveQuestion(SurveyQuestion q) => _questions.Remove(q);

        private void MoveUp(SurveyQuestion q)
        {
            int i = _questions.IndexOf(q);
            if (i > 0) (_questions[i], _questions[i - 1]) = (_questions[i - 1], _questions[i]);
        }

        private void MoveDown(SurveyQuestion q)
        {
            int i = _questions.IndexOf(q);
            if (i < _questions.Count - 1) (_questions[i], _questions[i + 1]) = (_questions[i + 1], _questions[i]);
        }

        private async Task Publish()
        {
            if (_questions.Any(q => string.IsNullOrWhiteSpace(q.Text))) return;
            await OnSurveyReady.InvokeAsync(_questions);
        }
    }
}

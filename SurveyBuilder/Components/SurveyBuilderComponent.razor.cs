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
        [Parameter] public List<SurveyQuestion> Questions { get; set; }  = new();
        private string _surveyTitle = "My Survey";

        protected override void OnParametersSet() => _surveyTitle = Title;

        private void AddQuestion() =>
            Questions.Add(new SurveyQuestion { Text = $"Question {Questions.Count + 1}" });

        private void RemoveQuestion(SurveyQuestion q) => Questions.Remove(q);

        private void MoveUp(SurveyQuestion q)
        {
            int i = Questions.IndexOf(q);
            if (i > 0) (Questions[i], Questions[i - 1]) = (Questions[i - 1], Questions[i]);
        }

        private void MoveDown(SurveyQuestion q)
        {
            int i = Questions.IndexOf(q);
            if (i < Questions.Count - 1) (Questions[i], Questions[i + 1]) = (Questions[i + 1], Questions[i]);
        }

        private async Task Publish()
        {
            if (Questions.Any(q => string.IsNullOrWhiteSpace(q.Text))) return;
            await OnSurveyReady.InvokeAsync(Questions);
        }
    }
}

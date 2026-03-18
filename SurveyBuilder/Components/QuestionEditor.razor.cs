using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SurveyBuilder.Models;


namespace SurveyBuilder.Components
{
    public partial class QuestionEditor
    {
        [Parameter] public SurveyQuestion Question { get; set; } = default!;
        [Parameter] public EventCallback OnRemove { get; set; }
        [Parameter] public EventCallback OnChanged { get; set; }

        private string _newOption = string.Empty;

        private async Task OnTypeChanged(ChangeEventArgs e)
        {
            if (Enum.TryParse<QuestionType>(e.Value?.ToString(), out var t))
            {
                Question.Type = t;
                Question.Options.Clear();
                await OnChanged.InvokeAsync();
            }
        }

        private async Task AddOption()
        {
            var opt = _newOption.Trim();
            if (!string.IsNullOrEmpty(opt) && !Question.Options.Contains(opt))
            {
                Question.Options.Add(opt);
                _newOption = string.Empty;
                await OnChanged.InvokeAsync();
            }
        }

        private async Task RemoveOption(string opt)
        {
            Question.Options.Remove(opt);
            await OnChanged.InvokeAsync();
        }

        private void HandleKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter") _ = AddOption();
        }
    }
}

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        [Parameter] public List<SurveyQuestion> Questions { get; set; } = new();
        private string _surveyTitle = "My Survey";

        // Drag state
        private SurveyQuestion? _draggedItem;
        private SurveyQuestion? _dragOverItem;
        private bool _isDraggingOver;

        protected override void OnParametersSet() => _surveyTitle = Title;

        private void AddQuestion() =>
            Questions.Add(new SurveyQuestion { Text = $"Question {Questions.Count + 1}" });

        private void RemoveQuestion(SurveyQuestion q) => Questions.Remove(q);

        // ---- Drag handlers ----
        private void OnDragStart(DragEventArgs e, SurveyQuestion q)
        {
            _draggedItem = q;
            _dragOverItem = null;
        }

        private void OnDragOver(DragEventArgs e, SurveyQuestion q)
        {
            if (_draggedItem is null || _draggedItem == q) return;
            _dragOverItem = q;
        }

        private void OnDragLeave(SurveyQuestion q)
        {
            if (_dragOverItem == q) _dragOverItem = null;
        }

        private void OnDrop(SurveyQuestion target)
        {
            if (_draggedItem is null || _draggedItem == target) { Reset(); return; }

            int from = Questions.IndexOf(_draggedItem);
            int to = Questions.IndexOf(target);

            if (from < 0 || to < 0) { Reset(); return; }

            Questions.RemoveAt(from);
            Questions.Insert(to, _draggedItem);
            Reset();
        }

        private void OnDragEnd() => Reset();

        private void Reset()
        {
            _draggedItem = null;
            _dragOverItem = null;
        }

        private string CardClass(SurveyQuestion q)
        {
            var cls = q.FullWidth ? "col-full" : "col-half";
            if (q == _draggedItem) cls += " dragging";
            if (q == _dragOverItem) cls += " drag-over";
            return cls;
        }

        private async Task Publish()
        {
            if (Questions.Any(q => string.IsNullOrWhiteSpace(q.Text))) return;
            await OnSurveyReady.InvokeAsync(Questions);
        }
    }
}

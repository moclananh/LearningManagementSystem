using Domain.Base;

namespace Domain.Entities
{
    public class QuizzQuestion : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }
    }
}

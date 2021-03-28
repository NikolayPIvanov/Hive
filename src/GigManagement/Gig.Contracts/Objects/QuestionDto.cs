namespace Hive.Gig.Domain.Objects
{
    public class QuestionDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Answer { get; set; }

        public int GigId { get; set; }
    }
}
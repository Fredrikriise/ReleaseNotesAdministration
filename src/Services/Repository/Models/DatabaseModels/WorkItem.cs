namespace Services.Repository.Models.DatabaseModels
{
    public class WorkItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssignedTo { get; set; }
        public string State { get; set; }

        public void AddWorkItemId(int workItemId)
        {
            Id = workItemId;
        }
    }
}

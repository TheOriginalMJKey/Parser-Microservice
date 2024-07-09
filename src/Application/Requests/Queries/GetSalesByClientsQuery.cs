namespace Application.Requests.Queries
{
    public class GetSalesByClientsQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ClientName { get; set; }
    }
}
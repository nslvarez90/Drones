namespace Drones.Models
{   
    public class Dron
    {
        public int Id { get; set; }
        public string serialNumber { get; set; }
        public string model { get; set; }      
        public float batteryCapacity { get; set; }
        public string status { get; set; }
        public DateTime createAt { get; set; }

        public  int maxWeight { get; set; }

        public List<Medicine> medicines { get; set; } = new List<Medicine>();


    }
}

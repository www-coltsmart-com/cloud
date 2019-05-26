using ColtSmart.Data;

namespace ColtSmart.Entity
{
    public class DeviceNet
    {
        [Key]
        public int id { get; set; }

        public int DeviceId { get; set; }

        public decimal NetFlow { get; set; }
    }
}

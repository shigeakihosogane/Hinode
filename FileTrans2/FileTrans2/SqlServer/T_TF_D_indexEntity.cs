using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTrans2.SqlServer
{
    public sealed class T_TF_D_indexEntity
    {

        public T_TF_D_indexEntity(
            int id,
            decimal zyutyuuid,
            DateTime kaisidate,
            DateTime syuuryoudate,
            string tanntoubusyo,
            string bikou,
            int ninusiid,
            string ninusimei,
            DateTime nyuuryokudate)
        {
            Id = id;
            ZyutyuuId = zyutyuuid;
            KaisiDate = kaisidate;
            SyuuryouDate = syuuryoudate;
            TanntouBusyo = tanntoubusyo;
            Bikou = bikou;
            NinusiId = ninusiid;
            NinusiMei = ninusimei;
            NyuuryokuDate = nyuuryokudate;
        }

        public int Id { get; }
        public decimal ZyutyuuId { get; }
        public DateTime KaisiDate { get; }
        public DateTime SyuuryouDate { get; }
        public string TanntouBusyo { get; }
        public string Bikou { get; }
        public int NinusiId { get; }
        public string NinusiMei { get; }
        public DateTime NyuuryokuDate{ get; }
    }
}

using System.Threading.Tasks;

namespace Servisi
{
    public interface IOgranicenjaKlijent
    {
        Task<OgranicenjaKonkursaKlasa> DajOgranicenjaAsync();
    }
}
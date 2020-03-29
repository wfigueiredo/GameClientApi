using GameProducer.Domain.Model;
using System.Threading.Tasks;

namespace GameProducer.Interfaces.Validators
{
    public interface IValidator<T> where T : BasePayload
    {
        void Validate(T t, params string[] p);
    }
}

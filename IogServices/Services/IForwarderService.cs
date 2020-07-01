using System.Net;
using System.Net.Http;
using IogServices.Models.DTO;

namespace IogServices.Services
{
    public interface IForwarderService<in T>
    {
        HttpResponseMessage ForwardCreation(T t);
        HttpResponseMessage ForwardEdition(T t);
        HttpResponseMessage ForwardDeletion(T t);
    }
}
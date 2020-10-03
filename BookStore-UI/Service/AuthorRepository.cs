using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BookStore_UI.Contracts;
using BookStore_UI.Models;

namespace BookStore_UI.Service {
    public class AuthorRepository : BaseRepository<Author>, IAuthorRepository {
        public AuthorRepository(IHttpClientFactory client, ILocalStorageService localStorage) : base(client, localStorage) { }
    }
}
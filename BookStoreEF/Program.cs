using BookStoreEF.Data;
using BookStoreEF.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStoreEF;

class Program
{
    static async Task Main(string[] args)
    {
        await BookStoreManager.Open();
    }
}

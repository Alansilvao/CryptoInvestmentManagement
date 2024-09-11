using Application.Dtos.Requests.Assets;
using Application.Exceptions;
using Application.Interfaces.Repositories;
using Domain.Entities;
using Infra.Database.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infra.Database.Repositories;

[ExcludeFromCodeCoverage]
public class AssetsRepository : IAssetsRepository
{
    private readonly ApplicationDbContext _context;

    public AssetsRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> PostAssetsAsync(string symbol, string name, int availableQuantity, decimal price)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(c => c.Symbol == symbol);

        if (asset is not null)
            throw new HttpStatusException(StatusCodes.Status400BadRequest, "Existing asset symbol");

        var newAsset = new Asset(symbol, name, availableQuantity, price);

        await _context.AddAsync(newAsset);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAssetsAsync(int id)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(c => c.Id == id);

        if (asset is null)
            throw new HttpStatusException(StatusCodes.Status404NotFound, "Asset not found");

        _context.Remove(asset);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Asset>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _context.Assets.ToListAsync(cancellationToken);

    public async Task<Asset> GetBySymbolAsync(string symbol, CancellationToken cancellationToken = default)
        => await _context.Assets.FirstOrDefaultAsync(it => it.Symbol == symbol, cancellationToken);

    public async Task<bool> PutAssetsAsync(PutAssetsRequest assets)
    {
        var asset = await _context.Assets.FirstOrDefaultAsync(c => c.Id == assets.Id);

        if (asset is null)
            throw new HttpStatusException(StatusCodes.Status404NotFound, "Asset not found");

        asset.Symbol = assets.Symbol;
        asset.Name = assets.Name;
        asset.AvailableQuantity = assets.AvailableQuantity;
        asset.Price = assets.Price;

        _context.Update(asset);
        await _context.SaveChangesAsync();

        return true;
    }
}
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SmartBurguerValueAPI.Context;
using SmartBurguerValueAPI.DTOs;
using SmartBurguerValueAPI.DTOs.Products;
using SmartBurguerValueAPI.Interfaces;
using SmartBurguerValueAPI.Models;
using SmartBurguerValueAPI.Models.Products;
using SmartBurguerValueAPI.Pagination;
using SmartBurguerValueAPI.Repository.Base;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace SmartBurguerValueAPI.Repository
{
    public class PurchaseRepository : RepositoryBase<PurchaseEntity>, IPurchaseRepository
    {
        private readonly IUnityOfWork _UnityOfWork;
        private readonly IMapper _map;

        public PurchaseRepository(AppDbContext context, IUnityOfWork unityOfWork, IMapper map) : base(context)
        {
            _UnityOfWork = unityOfWork;
            _map = map;
        }
        public async Task<List<PurchaseDTO>> GetAllPurchasesByEnterpriseId(Guid enterpriseId)
        {
            var query = await _context.Purchase
                .Where(x => x.EnterpriseId == enterpriseId)
                .OrderBy(x => x.Id)
                .ToListAsync();
            return _map.Map<List<PurchaseDTO>>(query);
        }
        public async Task<PurchaseDTO> GetPurchaseById(Guid purchaseId)
        {
            var purchase = _context.Purchase
                .Where(x => x.Id == purchaseId)
                .Include(x => x.Items)
                .ThenInclude(x => x.UnityOfMensure)
                .FirstOrDefault();
            return _map.Map<PurchaseDTO>(purchase);
        }

        public async Task<PurchaseEntity> CreatePurchase(PurchaseDTO dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            var purchaseId = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid();

            var entity = new PurchaseEntity
            {
                Id = purchaseId,
                SupplierName = dto.SupplierName,
                PurchaseDate = dto.PurchaseDate,
                TotalAmount = dto.TotalAmount,
                IsActive = dto.IsActive,
                DateCreated = dto.DateCreated != default ? dto.DateCreated : DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                EnterpriseId = dto.EnterpriseId,
                Items = dto.Items.Select(i => new PurchaseItemEntity
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchaseId,
                    NameItem = i.NameItem,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    IngredientId = i.IngredientId,
                    UnityOfMensureId = i.UnityOfMensureId,
                    InventoryItemId = i.InventoryItemId,
                    IsActive = true,
                }).ToList()
            };

            _context.Purchase.Add(entity);
            await _context.SaveChangesAsync();

            await UpdateProductsAsync(dto.EnterpriseId, dto);
            await UpdateComboAsync(dto.EnterpriseId, dto);
            return entity;
        }

        //    public async Task<PurchaseEntity> CreatePurchaseByXml(IFormFile file, Guid enterpriseId)
        //    {
        //        Console.WriteLine("========== INICIANDO PROCESSAMENTO DO XML ==========");

        //        using var reader = new StreamReader(file.OpenReadStream());
        //        var xmlContent = await reader.ReadToEndAsync();
        //        Console.WriteLine("XML carregado. Tamanho: " + xmlContent.Length);

        //        var document = XDocument.Parse(xmlContent);

        //        var supplierName = document.Descendants().FirstOrDefault(e => e.Name.LocalName == "xNome")?.Value?.Trim() ?? "Fornecedor Desconhecido";
        //        Console.WriteLine($"Fornecedor identificado: {supplierName}");

        //        var dateStr = document.Descendants().FirstOrDefault(e => e.Name.LocalName == "dhEmi")?.Value;
        //        if (!DateTime.TryParse(dateStr, null, System.Globalization.DateTimeStyles.AdjustToUniversal, out var purchaseDate))
        //            purchaseDate = DateTime.UtcNow;
        //        Console.WriteLine($"Data da compra: {purchaseDate}");

        //        var totalStr = document.Descendants().FirstOrDefault(e => e.Name.LocalName == "vNF")?.Value ?? "0";
        //        decimal.TryParse(totalStr, NumberStyles.Any, CultureInfo.InvariantCulture, out var totalAmount);
        //        Console.WriteLine($"Valor total da compra: {totalAmount}");

        //        var purchase = new PurchaseEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            SupplierName = supplierName,
        //            PurchaseDate = purchaseDate,
        //            TotalAmount = totalAmount,
        //            EnterpriseId = enterpriseId,
        //            DateCreated = DateTime.UtcNow,
        //            DateUpdated = DateTime.UtcNow,
        //            IsActive = true,
        //            Items = new List<PurchaseItemEntity>()
        //        };

        //        var items = document.Descendants().Where(e => e.Name.LocalName == "det").ToList();
        //        Console.WriteLine($"Itens encontrados no XML: {items.Count}");

        //        foreach (var item in items)
        //        {
        //            var name = item.Descendants().FirstOrDefault(e => e.Name.LocalName == "xProd")?.Value?.Trim();
        //            if (string.IsNullOrWhiteSpace(name)) continue;

        //            var quantity = decimal.TryParse(item.Descendants().FirstOrDefault(e => e.Name.LocalName == "qCom")?.Value ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out var q) ? q : 0;
        //            var price = decimal.TryParse(item.Descendants().FirstOrDefault(e => e.Name.LocalName == "vUnCom")?.Value ?? "0", NumberStyles.Any, CultureInfo.InvariantCulture, out var p) ? p : 0;
        //            var unitSymbol = Regex.Replace(item.Descendants().FirstOrDefault(e => e.Name.LocalName == "uCom")?.Value?.Trim().ToUpper() ?? "UN", @"\s+", "");

        //            var unitOfMeasure = await FindOrCreateStandardUnitAsync(unitSymbol, quantity);

        //            //var ingredient = await _context.Set<IngredientsEntity>()
        //            //    .Include(i => i.UnitOfMeasure)
        //            //    .FirstOrDefaultAsync(i => i.Name.Trim().Equals(name.Trim(), StringComparison.OrdinalIgnoreCase) && i.EnterpriseId == enterpriseId);
        //            var ingredient = await _context.Set<IngredientsEntity>()
        //            .Include(i => i.UnitOfMeasure)
        //.FirstOrDefaultAsync(i =>
        //    EF.Functions.Like(i.Name, name) && i.EnterpriseId == enterpriseId);


        //            if (ingredient == null)
        //            {
        //                var inventoryItem = new InventoryItemEntity
        //                {
        //                    Id = Guid.NewGuid(),
        //                    Name = name,
        //                    NameCategory = "Ingredient",
        //                    UnityOfMensureId = unitOfMeasure.Id,
        //                    EnterpriseId = enterpriseId,
        //                    DateCreated = DateTime.UtcNow,
        //                    DateUpdated = DateTime.UtcNow,
        //                    IsActive = true
        //                };
        //                await _context.Set<InventoryItemEntity>().AddAsync(inventoryItem);

        //                ingredient = new IngredientsEntity
        //                {
        //                    Id = Guid.NewGuid(),
        //                    Name = name,
        //                    UnitOfMeasureId = unitOfMeasure.Id,
        //                    InventoryItemId = inventoryItem.Id,
        //                    EnterpriseId = enterpriseId,
        //                    DateCreated = DateTime.UtcNow,
        //                    DateUpdated = DateTime.UtcNow,
        //                    IsActive = true
        //                };
        //                await _context.Set<IngredientsEntity>().AddAsync(ingredient);
        //            }

        //            purchase.Items.Add(new PurchaseItemEntity
        //            {
        //                Id = Guid.NewGuid(),
        //                NameItem = name,
        //                PurchaseId = purchase.Id,
        //                InventoryItemId = ingredient.InventoryItemId,
        //                UnityOfMensureId = unitOfMeasure.Id,
        //                Quantity = quantity,
        //                UnitPrice = price,
        //                DateCreated = DateTime.UtcNow,
        //                DateUpdated = DateTime.UtcNow,
        //                IsActive = true
        //            });
        //        }

        //        await _context.Set<PurchaseEntity>().AddAsync(purchase);
        //        await _context.SaveChangesAsync();

        //        Console.WriteLine("========== FIM DO PROCESSAMENTO ==========");
        //        return purchase;
        //    }

        //    private async Task<UnityTypesProductsEntity> FindOrCreateStandardUnitAsync(string symbol, decimal quantity)
        //    {
        //        symbol = symbol.Trim().ToUpperInvariant();
        //        Console.WriteLine($"🔎 Procurando unidade: {symbol}");

        //        // Tenta encontrar unidade existente
        //        var existing = await _context.Set<UnityTypesProductsEntity>()
        //            .FirstOrDefaultAsync(u => u.Symbol == symbol && u.IsActive);
        //        if (existing != null)
        //        {
        //            Console.WriteLine($"✅ Unidade encontrada no banco: {existing.Symbol} (ID: {existing.Id})");
        //            return existing;
        //        }

        //        Console.WriteLine($"⚠️ Unidade '{symbol}' não encontrada. Criando nova...");

        //        // Define unidade base e fator de conversão
        //        string baseSymbol = "UN";
        //        decimal conversion = 1m;

        //        // CX, PCT ou formatos similares: CX12, PCT5
        //        var match = Regex.Match(symbol, @"^(CX|PCT)(\d+)$");
        //        if (match.Success)
        //        {
        //            conversion = Convert.ToDecimal(match.Groups[2].Value);
        //        }
        //        else
        //        {
        //            // Outras unidades comuns
        //            switch (symbol)
        //            {
        //                case "KG": baseSymbol = "KG"; conversion = 1m; break;
        //                case "G": baseSymbol = "KG"; conversion = 0.001m; break;
        //                case "L": baseSymbol = "L"; conversion = 1m; break;
        //                case "ML": baseSymbol = "L"; conversion = 0.001m; break;
        //                default:
        //                    baseSymbol = "UN"; conversion = 1m; break;
        //            }
        //        }

        //        // Busca unidade base
        //        var baseUnit = await _context.Set<UnityTypesProductsEntity>()
        //            .FirstOrDefaultAsync(u => u.Symbol == baseSymbol && u.IsActive);

        //        if (baseUnit == null)
        //        {
        //            Console.WriteLine($"❌ ERRO: Unidade base '{baseSymbol}' não existe no banco!");
        //            throw new Exception($"Unidade base '{baseSymbol}' não encontrada no banco. Cadastre-a primeiro.");
        //        }

        //        // Cria nova unidade
        //        var newUnit = new UnityTypesProductsEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = symbol,
        //            Symbol = symbol,
        //            BaseUnit = baseUnit.Symbol,
        //            ConversionFactor = conversion,
        //            DateCreated = DateTime.UtcNow,
        //            DateUpdated = DateTime.UtcNow,
        //            IsActive = true
        //        };

        //        await _context.Set<UnityTypesProductsEntity>().AddAsync(newUnit);
        //        // Não faz SaveChanges aqui, deixe que o método chamador salve tudo junto no final
        //        Console.WriteLine($"🆕 Unidade criada: {symbol} | Base: {baseSymbol} | Fator: {conversion}");

        //        return newUnit;
        //    }

        public async Task UpdatePurchaseAsync(PurchaseDTO dto)
        {
            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(p => p.Id == dto.Id);

            if (purchase == null)
                throw new Exception("Compra não encontrada.");

            decimal total = 0;

            var oldItems = await _context.PurchaseItem
                .Where(pi => pi.PurchaseId == purchase.Id)
                .ToListAsync();

            _context.PurchaseItem.RemoveRange(oldItems);

            foreach (var item in dto.Items)
            {
                var purchaseItem = new PurchaseItemEntity
                {
                    Id = Guid.NewGuid(),
                    PurchaseId = purchase.Id,
                    IngredientId = item.IngredientId,
                    InventoryItemId = item.InventoryItemId,
                    UnityOfMensureId = item.UnityOfMensureId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    NameItem = item.NameItem,
                    IsActive = true
                };

                total += item.Quantity * item.UnitPrice;

                await _context.PurchaseItem.AddAsync(purchaseItem);
            }

            purchase.SupplierName = dto.SupplierName;
            purchase.PurchaseDate = dto.PurchaseDate;
            purchase.IsActive = dto.IsActive;
            purchase.TotalAmount = total;
            purchase.DateUpdated = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductsAsync(Guid enterpriseId, PurchaseDTO purchase)
        {
            var products = await _UnityOfWork.ProductRepository.GetAllProductsByEnterpriseId(enterpriseId);

            foreach (var product in products)
            {
                if (product.Ingredients == null || !product.Ingredients.Any())
                    continue;

                var matchedIngredients = product.Ingredients
                    .Where(ingredient => purchase.Items
                        .Any(purchaseItem => purchaseItem.IngredientId == ingredient.Id))
                    .ToList();
                if (purchase.PurchaseDate > DateTime.Now)
                {
                    continue;
                }
                if (matchedIngredients != null)
                {
                    await _UnityOfWork.ProductRepository.UpdateProductAsync(product);
                }

            }

        }
        public async Task UpdateComboAsync(Guid enterpriseId, PurchaseDTO purchase)
        {
            var combos = await _UnityOfWork.ComboRepository.GetAllCombosByEnterpriseIdAsync(enterpriseId);

            foreach (var combo in combos)
            {
                if (combo.Products == null || !combo.Products.Any())
                    continue;

                foreach (var product in combo.Products)
                {
                    if (product.Ingredients == null || !product.Ingredients.Any())
                        continue;

                    foreach (var ingredient in product.Ingredients)
                    {
                        var purchaseItem = purchase.Items
                            .Where(p => p.IngredientId == ingredient.Id);
                        if (purchase.PurchaseDate > DateTime.Now)
                        {
                            continue;
                        }
                        if (purchaseItem != null)
                        {
                            await _UnityOfWork.ComboRepository.UpdateComboAsync(combo);
                        }
                    }
                }

            }

        }

    }
}

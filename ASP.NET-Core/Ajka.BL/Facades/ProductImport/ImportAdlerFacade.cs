using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Ajka.BL.Facades.ProductImport.Interfaces;
using Ajka.BL.Services.Base.Interfaces;
using System.Xml;
using System.Diagnostics;
using Ajka.BL.Models.ProductImport;
using Ajka.BL.Services.Category.Interfaces;
using Ajka.BL.Services.ItemCard.Interfaces;
using Ajka.Common.Helpers;

namespace Ajka.BL.Facades.ProductImport
{
    public class ImportAdlerFacade : IImportAdlerFacade, IAjkaShopService
    {
        private readonly ICategoryService _categoryService;
        private readonly IItemCardService _itemCardService;
        private readonly IErrorLogService _errorLogService;

        public ImportAdlerFacade(ICategoryService categoryService,
                                 IItemCardService itemCardService,
                                 IErrorLogService errorLogService)
        {
            _categoryService = categoryService;
            _itemCardService = itemCardService;
            _errorLogService = errorLogService;
        }

        public async Task ImportAsync(string productsData, CancellationToken cancellationToken)
        {
            try
            {
                var productsToImport = new List<ImportAdlerDto>();
                var document = new XmlDocument();
                document.LoadXml(productsData);
                var productList = document.GetElementsByTagName("product");
                foreach (XmlNode product in productList)
                {
                    XmlNode child = product.SelectSingleNode("childs");
                    var sexList = new List<string>();
                    XmlNode sex = child.SelectSingleNode("sex");
                    foreach (XmlNode sexId in sex.SelectSingleNode("childs"))
                    {
                        sexList.Add(sexId.InnerText);
                    }
                    var productToAdd = new ImportAdlerDto
                    {
                        CategoryId = child.SelectSingleNode("id_category")?.InnerText,
                        CategoryName = child.SelectSingleNode("category")?.InnerText,
                        ProductName = child.SelectSingleNode("name")?.InnerText,
                        ProductLabel = child.SelectSingleNode("label")?.InnerText,
                        Description = child.SelectSingleNode("description")?.InnerText,
                        SizeName = child.SelectSingleNode("name_size")?.InnerText,
                        ColorName = child.SelectSingleNode("name_color")?.InnerText,
                        SizeId = child.SelectSingleNode("id_size")?.InnerText,
                        Price = Convert.ToDecimal(child.SelectSingleNode("price")?.InnerText),
                        ProductImagePath = child.SelectSingleNode("product_image")?.InnerText,
                        Grammage = child.SelectSingleNode("grammage")?.InnerText,
                        Quantity = int.Parse(child.SelectSingleNode("expedition_package")?.InnerText),
                        SexList = sexList
                    };
                    productsToImport.Add(productToAdd);
                }

                if (productsToImport.Any())
                {
                    var categories = productsToImport.GroupBy(g => g.CategoryName).Select(x => x.Key);
                    await _categoryService.SynchronizeGroupCategoriesAsync(categories, cancellationToken).ConfigureAwait(false);

                    var itemCards = productsToImport.GroupBy(g => new { g.CategoryId, g.CategoryName, g.ProductName, g.ProductLabel })
                        .Select(item => new ImportAdlerDto
                        {
                            CategoryId = item.Key.CategoryId,
                            CategoryName = item.Key.CategoryName,
                            ProductName = item.Key.ProductName,
                            ProductLabel = item.Key.ProductLabel,
                            Description = item.Min(d => d.Description),
                            Quantity = item.Min(q => q.Quantity),
                            Price = item.Max(p => p.Price),
                            Grammage = item.Min(g => g.Grammage),
                            SexList = item.Select(s => s.SexList).FirstOrDefault(),
                            ImagePaths = item.GroupBy(g => new { g.ColorName, g.ProductImagePath })
                                .Select(select => new ImportAdlerImageDto
                                {
                                    ColorName = select.Key.ColorName,
                                    ImagePath = select.Key.ProductImagePath.Replace("xl.jpg", "l.jpg")
                                }).ToList(),
                            Sizes = item.GroupBy(g => new { g.SizeId, g.SizeName })
                                .Select(select => new ImportAdlerSizeDto
                                {
                                    SizeId = select.Key.SizeId,
                                    SizeName = select.Key.SizeName,
                                    Price = select.Max(p => p.Price)
                                }).ToList()
                        });
                    await _itemCardService.SynchronizeAdlerItemCardsAsync(itemCards, cancellationToken).ConfigureAwait(false);
                }
                Debug.WriteLine("Done");
            }
            catch (Exception ex)
            {
                await _errorLogService.LogExceptionAsync(ex).ConfigureAwait(false);
            }
        }
    }
}

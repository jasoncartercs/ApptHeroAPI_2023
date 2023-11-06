using ApptHeroAPI.Repositories.Context.Entities;
using ApptHeroAPI.Services.Abstraction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApptHeroAPI.Services.Implementation.Factory
{
    public interface IPersonPackageModelFactory
    {
        PersonPackageModel Create(PersonPackage personPackage);
        List<PersonPackageModel> CreateList(List<PersonPackage> personPackages);

    }

    public class PersonPackageModelFactory : IPersonPackageModelFactory
    {
        public PersonPackageModel Create(PersonPackage p)
        {
            if (p == null) throw new ArgumentNullException(nameof(p));

            var model = new PersonPackageModel
            {
                // Populate fields from personPackage...
                DateToSend = p.DateToSend,
                IsActive = p.IsActive,
                IsPurchasedForMe = p.IsPurchasedForMe,
                Message = p.Message,
                PackageId = p.PackageId,
                DatePurchased = p.DatePurchased,
                GiftCardCertificateNumber = p.GiftCardCertificateNumber,
                PersonPackageId = p.PersonPackageId
            };
            // Initialize nested properties conditionally
            if (p.Package != null)
            {
                model.PackageModel = new PackageModel
                {
                    CompanyId = p.Package.CompanyId,
                    ExpiresAfterDays = p.Package.ExpiresAfterDays,
                    ImageUrl = p.Package.ImageUrl,
                    IsGiftCertificate = p.Package.IsGiftCertificate,
                    PackageId = p.Package.PackageId,
                    ProductId = p.Package.ProductId,
                    RedeemableForId = p.Package.RedeemableForId,
                    TotalAmount = p.Package.TotalAmount,
                    TotalMinutes = p.Package.TotalMinutes,
                };

                if (p.Package.Product != null)
                {
                    model.PackageModel.ProductModel = new ProductModel
                    {
                        CompanyID = p.Package.CompanyId,
                        CreatedDate = p.Package.Product.CreatedDate,
                        Description = p.Package.Product.Description,
                        ImageUrl = p.Package.Product.ImageUrl,
                        ServiceName = p.Package.Product.Name,
                        Price = p.Package.Product.Price,
                        ProductId = p.Package.ProductId
                    };
                }

            }
            return model;
        }

        public List<PersonPackageModel> CreateList(List<PersonPackage> personPackages)
        {
            if (personPackages == null)
            {
                throw new ArgumentNullException(nameof(personPackages));
            }

            return personPackages.Select(Create).ToList();
        }
    }
}

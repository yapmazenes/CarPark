using AspNetCore.Identity.MongoDbCore.Models;
using CarPark.Business.Abstract;
using CarPark.Core.Models;
using CarPark.DataAccess.Abstract;
using CarPark.Entities.Concrete;
using CarPark.Models.ViewModels.Personels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.Business.Concrete
{
    public class PersonelManager : IPersonelService
    {
        private readonly IPersonelDataAccess _personelDataAccess;
        private readonly RoleManager<MongoIdentityRole> _roleManager;
        public PersonelManager(IPersonelDataAccess personelDataAccess, RoleManager<MongoIdentityRole> roleManager)
        {
            _personelDataAccess = personelDataAccess;
            _roleManager = roleManager;
        }

        public async Task<GetOneResult<PersonelMainRole>> GetPersonelRoles(string id)
        {
            var result = new GetOneResult<PersonelMainRole>();

            try
            {
                var roles = _roleManager.Roles?.ToList();

                var personel = await _personelDataAccess.GetByIdAsync(id, "guid");

                var personelRoles = personel?.Entity?.Roles?.Select(role =>
                                        new PersonelRoles
                                        {
                                            Id = role.ToString(),
                                            Name = roles?.FirstOrDefault(x => x.Id == role)?.Name ?? "UndefinedRoleName"
                                        }).ToList();

                result.Entity = new PersonelMainRole
                {
                    Roles = roles,
                    PersonelRoleList = personelRoles
                };

                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Success = false;
            }

            return result;
        }

        public GetManyResult<Personel> GetPersonelsByAge()
        {
            var personelList = _personelDataAccess.GetAll();
            return personelList;
        }

        public async Task<GetOneResult<Personel>> UpdatePersonelRoles(string personelId, string[] personelRoleList)
        {
            var personel = await _personelDataAccess.GetByIdAsync(personelId, "guid");

            if (personel != null)
            {
                var roles = personelRoleList.Select(roleId => new Guid(roleId)).ToList();

                personel.Entity.Roles = roles;

                var result = await _personelDataAccess.ReplaceOneAsync(personel.Entity, personelId, "guid");
                result.Message = $"{personel.Entity.Name} {personel.Entity.Surname}'s roles updated.";

                personel = result;
                personel.Success = true;
            }
            else
            {
                personel = new GetOneResult<Personel>();
                personel.Message = $"No user found with {personelId}.";
                personel.Success = false;
            }

            return personel;
        }
    }
}

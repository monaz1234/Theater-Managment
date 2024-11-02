﻿using AdminService.Entity.Model;
using AdminService.Repository.MongoDBRepo;

namespace AdminService.Repository.Interface;

public interface IMovieSaleRepository : IRepository<MovieSale>
{
    //xóa tất cả movie sale
    Task DeleteAll();
}
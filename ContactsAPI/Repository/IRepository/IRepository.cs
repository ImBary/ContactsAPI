﻿using System.Linq.Expressions;

namespace ContactsAPI.Repository.IRepository
{
	public interface IRepository<T> where T : class
	{
		//ogolny CRD dla T entity Update osobno 
		Task CreateAsync(T entity);
		Task SaveAsync();
		Task RemoveAsync(T entity);

		Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
		Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true, string? includeProperties = null);
	}
}
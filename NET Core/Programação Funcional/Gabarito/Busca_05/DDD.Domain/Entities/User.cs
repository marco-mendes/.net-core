using System;

namespace DDD.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Cpf { get; set; }
        public DateTime BirthDate { get; set; }
        public bool VerificaMaiorIdade (DateTime? nascimento = null) {
            DateTime comparar = (nascimento == null) ? this.BirthDate : nascimento ?? default(DateTime);
            if (((DateTime.Now - comparar).TotalDays / 365.25) > 18) {
            	return true;
            }
        	return false;
        }
    }
}
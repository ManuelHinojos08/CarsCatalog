using CarsCatalog.DDL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.DDL
{
    public class UnitOfWork : IDisposable
    {
        private CarCatalogEntities context = new CarCatalogEntities();
        private UsersRepository usersRepository;
        private AdsRepository adsRepository;
        private ImagesRepository imagesRepository;

        public UsersRepository UsersRepository
        {
            get
            {

                if (this.usersRepository == null)
                {
                    this.usersRepository = new UsersRepository(context);
                }
                return usersRepository;
            }
        }

        public AdsRepository AdsRepository
        {
            get
            {

                if (this.adsRepository == null)
                {
                    this.adsRepository = new AdsRepository(context);
                }
                return adsRepository;
            }
        }

        public ImagesRepository ImagesRepository
        {
            get
            {

                if (this.imagesRepository == null)
                {
                    this.imagesRepository = new ImagesRepository(context);
                }
                return imagesRepository;
            }
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

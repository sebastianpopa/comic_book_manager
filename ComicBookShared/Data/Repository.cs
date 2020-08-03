using ComicBookShared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace ComicBookShared.Data
{
    public class Repository
    {
        private Context _context = null;

        public Repository(Context context)
        {
            this._context = context;
        }

        public IList<ComicBook> GetComicBooks()
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .OrderBy(cb => cb.Series.Title)
                    .ThenBy(cb => cb.IssueNumber)
                    .ToList();
        }

        public ComicBook GetComicBook(int id)
        {
            return _context.ComicBooks
                    .Include(cb => cb.Series)
                    .Include(cb => cb.Artists.Select(a => a.Artist))
                    .Include(cb => cb.Artists.Select(a => a.Role))
                    .Where(cb => cb.Id == id)
                    .SingleOrDefault();
        }

        public IList<Artist> GetArtists()
        {
            return _context.Artists
                .OrderBy(x => x.Id)
                .ToList();
        }

        public IList<Role> GetRoles()
        {
            return _context.Roles
                .OrderBy(x => x.Id)
                .ToList();
        }

        public IList<Series> GetSeries()
        {
            return _context.Series
                .OrderBy(x => x.Id)
                .ToList();
        }

        public void AddComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            _context.SaveChanges();
        }

        public void EditComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteComicBook(ComicBook comicBook)
        {
            _context.Entry(comicBook).State = EntityState.Deleted;
            _context.SaveChanges();
        }
        public IList<ComicBookArtist> GetComicBookArtists()
        {
            return _context.ComicBookArtists.OrderBy(a => a.Role.Name).ToList();
        }
        public ComicBookArtist GetComicBookArtist(int id)
        {
            return _context.ComicBookArtists
                .Include(cba => cba.ComicBook)
                .Include(cba => cba.ComicBook.Series)
                .Include(cba => cba.Artist)
                .Include(cba => cba.Role)
                .Where(cba => cba.Id == id)
                .SingleOrDefault();
        }

        public void AddComicBookArtist(ComicBookArtist comicBookArtist)
        {
            _context.ComicBookArtists.Add(comicBookArtist);
            _context.SaveChanges();
        }

        public void DeleteComicBookArtist(int id)
        {
            _context.Entry(new ComicBookArtist() { Id = id }).State = EntityState.Deleted;
            _context.SaveChanges();
        }
    }
}

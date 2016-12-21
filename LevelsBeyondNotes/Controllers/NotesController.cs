using LevelsBeyondNotes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LevelsBeyondNotes.Controllers
{
    public class NotesController : ApiController
    {
        // GET: api/Note
        public IEnumerable<Note> Get()
        {
            return NotesRepo.GetNotes(null);
        }

        // GET: api/Note?query=whatever
        public IEnumerable<Note> Get([FromUri] string query)
        {
            return NotesRepo.GetNotes(query);
        }

        // GET: api/Note/5
        public Note Get(int id)
        {
            return NotesRepo.GetNoteById(id);
        }

        // POST: api/Note
        public Note Post([FromBody]Note noteBody)
        {
            NotesRepo.CreateNote(noteBody);
            return noteBody;
        }
    }
}

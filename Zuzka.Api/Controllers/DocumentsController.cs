using Microsoft.AspNetCore.Mvc;
using Zuzka.Services.Contracts;
using Zuzka.Services.DTO;
using Zuzka.Services.Exceptions;

namespace CodeRama.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocumentsController : BaseController<DocumentsController>
    {
        private readonly IDocumentService _documentService;
        public DocumentsController(ILogger<DocumentsController> logger, IDocumentService documentService) : base(logger)
        {
            _documentService = documentService;
        }

        [HttpGet("{documentId}")]
        [Produces("application/json", "application/xml")]
        public async Task<ActionResult<DocumentDto>> GetDocumentById(Guid documentId)
        {
            try
            {
                var document = await _documentService.GetDocumentByIdAsync(documentId);

                if (document != null)
                {
                    return Ok(document);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                base.LogError($"Exception thrown {nameof(GetDocumentById)} for documentId {documentId}, exception: {ex}");
                return StatusCode(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateDocument([FromBody] DocumentRequestDto document)
        {
            try
            {
                var createdId = await _documentService.CreateDocumentAsync(document);
                return Ok(createdId.ToString());
            }
            catch (ValidationException ex)
            {
                base.LogCritical($"{nameof(ValidationException)} thrown {nameof(GetDocumentById)} for documentId {document.DocumentId}, exception: {ex}");
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError($"Exception thrown {nameof(GetDocumentById)} for documentId {document.DocumentId}, exception: {ex}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult> EditDocument([FromBody] DocumentRequestDto document)
        {
            try
            {
                var editedId = await _documentService.UpdateDocumentAsync(document);
                return Ok(editedId.ToString());
            }
            catch (ValidationException ex)
            {
                base.LogCritical($"{nameof(ValidationException)} thrown {nameof(GetDocumentById)} for documentId {document.DocumentId}, exception: {ex}");
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError($"Exception thrown {nameof(GetDocumentById)} for documentId {document.DocumentId}, exception: {ex}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}

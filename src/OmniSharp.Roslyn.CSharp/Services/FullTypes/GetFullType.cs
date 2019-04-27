using System.Composition;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.Text;
using OmniSharp.Mef;
using OmniSharp.Models.GetFullType;
using OmniSharp.Options;
using OmniSharp.Roslyn.CSharp.Services.Documentation;

namespace OmniSharp.Roslyn.CSharp.Services.FullTypes
{
    [OmniSharpHandler(OmniSharpEndpoints.GetFullType, LanguageNames.CSharp)]
    public class FullTypeService : IRequestHandler<GetFullTypeRequest, GetFullTypeResponse>
    {
        private readonly OmniSharpWorkspace _workspace;
        private readonly FormattingOptions _formattingOptions;

        [ImportingConstructor]
        public FullTypeService(OmniSharpWorkspace workspace, FormattingOptions formattingOptions)
        {
            _workspace = workspace;
            _formattingOptions = formattingOptions;
        }

        public async Task<GetFullTypeResponse> Handle(GetFullTypeRequest request)
        {
                       var document = _workspace.GetDocument(request.FileName);
            var response = new GetFullTypeResponse();
            if (document != null)
            {
                var semanticModel = await document.GetSemanticModelAsync();
                var sourceText = await document.GetTextAsync();
                var position = sourceText.Lines.GetPosition(new LinePosition(request.Line, request.Column));
                var symbol = await SymbolFinder.FindSymbolAtPositionAsync(semanticModel, position, _workspace);
                if (symbol != null)
                {
                    response.Type = symbol.ToDisplayString();
                }
            }

            return response;
        }
    }
}

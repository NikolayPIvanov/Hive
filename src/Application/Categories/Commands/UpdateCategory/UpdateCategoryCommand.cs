using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Hive.Application.Common.Exceptions;
using Hive.Application.Common.Interfaces;
using Hive.Application.Common.Mappings;
using Hive.Domain.Entities.Categories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Hive.Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest, IMapFrom<Category>
    {
        public UpdateCategoryCommand()
        {
            SubCategoriesIds = new();
        }
        
        public int Id { get; set; }
        
        public string Title { get; set; }

        public int? ParentCategoryId { get; set; }

        public List<int> SubCategoriesIds { get; private set; }
    }
    
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateCategoryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                throw new NotFoundException(nameof(Category), request.Id);
            }
            
            var updatedSubCategories = _context.Categories.Where(c => request.SubCategoriesIds.Contains(c.Id));

            entity.Title = request.Title;
            entity.ParentCategoryId = request.ParentCategoryId;
            entity.SubCategories = await updatedSubCategories.ToListAsync(cancellationToken);

            return  Unit.Value;
        }
    }
}
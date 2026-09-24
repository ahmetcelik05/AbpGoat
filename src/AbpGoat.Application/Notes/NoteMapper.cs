using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;

namespace AbpGoat.Notes;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatNoteToNoteDtoMapper : MapperBase<Note, NoteDto>
{
    public override partial NoteDto Map(Note source);

    public override partial void Map(Note source, NoteDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AbpGoatCreateUpdateNoteDtoToNoteMapper : MapperBase<CreateUpdateNoteDto, Note>
{
    public override partial Note Map(CreateUpdateNoteDto source);

    public override partial void Map(CreateUpdateNoteDto source, Note destination);
}

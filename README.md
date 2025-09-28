- Event Sourcing basics
- Custom projection
- Event replay

- Alimentação: busca por pequenos animais, ovos e frutas
- Exploração: percorre tocas, troncos e áreas abertas
- Descanso: dorme em locais protegidos durante o dia
- Higiene: limpa o pelo regularmente
1. Alimentação: busca por pequenos animais, ovos e frutas
2. Exploração: percorre tocas, troncos e áreas abertas
3. Interação social: brinca, comunica-se e coopera com outros membros do grupo
4. Higiene: limpa o pelo regularmente
5. Descanso: dorme em locais protegidos durante o dia
6. Defense: uses agility and strong odor for protection

```csharp
public record FeedingEvent(DateTime Timestamp, string Food);
public record ExplorationEvent(DateTime Timestamp, string ExploredLocation);
public record SocialInteractionEvent(DateTime Timestamp, string InteractionType, string OtherIndividual);
public record GroomingEvent(DateTime Timestamp, string Method);
public record RestEvent(DateTime Timestamp, string RestLocation);
public record DefenseEvent(DateTime Timestamp, string DefenseMethod);
```
- Defesa: utiliza agilidade e odor forte para se proteger
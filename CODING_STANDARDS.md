# TFX50 Coding Standards

Padrões para orientar a escrita de código C# (.NET9 / C#13) e TypeScript neste repositório. O foco é legibilidade, performance e consistência.

## Convenções de Nomenclatura

- Classes (C# e TS): inglês, prefixo `X`, `PascalCase`.
 - Ex.: `XUserService`, `XOrderProcessor`.
- Interfaces (C# e TS): inglês, prefixo `XI`, `PascalCase`.
 - Ex.: `XIRepository`, `XIUserStore`.
- Métodos e propriedades: `PascalCase`.
 - Ex.: `GetById`, `SaveChanges`, `LastAccessAt`.
- Campos (field) no corpo da classe: prefixo `_` seguido de `PascalCase`.
 - Ex.: `_Cache`, `_Repository`, `_MaxSize`.
- Parâmetros de métodos: `p` + `PascalCase`.
 - Ex.: `pUserID`, `pOptions`, `pToken`.
- Variáveis locais: mnemônicas, abreviadas, tudo em minúsculo.
 - Ex.: `lstua` (lista de usuários ativos), `frsrt` (first read table).
- Quando o nome original é abreviado, comp CEP, CPF, ID, URL, HTTP, JSON, XML, SQL, DB, UI, UX, deve manter caixa alta
 - Ex.: `pUserID`, `GetByURL`, `LoadFromDB`.
- Nomes sempre em inglês para tipos, membros e arquivos.

## Estilo de Código

- Blocos de uma única linha (ex.: `if`) não usam chaves.
- Evitar métodos anônimos e lambdas; preferir métodos nomeados.
- Não escrever comentários; o código deve ser autoexplicativo.
- Prefira retornos antecipados para reduzir níveis de aninhamento.
- Um tipo por arquivo, com o nome do arquivo igual ao da classe/inteface principal.

## Performance e Boas Práticas (C#)

- Evitar alocações desnecessárias e capturas de closures.
- Usar `Span`/`ReadOnlySpan` e `StringBuilder` em processamento intensivo de texto.
- Evitar LINQ em caminhos críticos; preferir loops explícitos.
- Tornar tipos `sealed` quando não houver necessidade de herança.
- Preferir `readonly` em campos/structs imutáveis e parâmetros `in` quando apropriado.
- Usar `Try*` e APIs `*OrDefault` para fluxos sem exceções como controle.

## Performance e Boas Práticas (TypeScript)

- Usar `const` e `let`; não usar `var`.
- Evitar `any`; preferir tipos explícitos, genéricos e unions.
- Preferir funções nomeadas em vez de funções anônimas/arrow em hot paths.
- Minimizar criações de objetos/arrays e encadeamentos desnecessários.
- Usar early-return e checagens simples para clareza e custo reduzido.

## Exemplos (C#)

```csharp
public interface XIUserRepository
{
 IEnumerable<XUser> GetActive(Boolean pIncludeAdmins);
}

public sealed class XUser
{
 public Guid Id { get; }
 public String Name { get; }
 public Boolean IsActive { get; }
 public Boolean IsAdmin { get; }

 public XUser(Guid pId, String pName, Boolean pIsActive, Boolean pIsAdmin)
 {
 Id = pId;
 Name = pName;
 IsActive = pIsActive;
 IsAdmin = pIsAdmin;
 }
}

public sealed class XUserService
{
 private readonly XIUserRepository _Repository;

 public XUserService(XIUserRepository pRepository)
 {
 _Repository = pRepository;
 }

 public IEnumerable<XUser> GetActiveUsers(Boolean pIncludeAdmins)
 {
 var lstua = _Repository.GetActive(pIncludeAdmins);
 if (lstua == null) return Array.Empty<XUser>();
 return FilterActive(lstua, pIncludeAdmins);
 }

 private static Boolean IsActive(XUser pUser)
 {
 if (!pUser.IsActive) return false;
 return true;
 }

 private static IEnumerable<XUser> FilterActive(IEnumerable<XUser> pUsers, Boolean pIncludeAdmins)
 {
 foreach (var usr in pUsers)
 {
 if (!IsActive(usr)) continue;
 if (!pIncludeAdmins && usr.IsAdmin) continue;
 yield return usr;
 }
 }
}
```

## Exemplos (TypeScript)

```ts
export interface XIUserRepository {
 GetActive(pIncludeAdmins: boolean): XUser[];
}

export class XUser {
 constructor(
 public readonly Id: string,
 public readonly Name: string,
 public readonly IsActive: boolean,
 public readonly IsAdmin: boolean
 ) {}
}

export class XUserService {
 private readonly _Repository: XIUserRepository;

 constructor(pRepository: XIUserRepository) {
 this._Repository = pRepository;
 }

 public GetActiveUsers(pIncludeAdmins: boolean): XUser[] {
 const lstua = this._Repository.GetActive(pIncludeAdmins);
 if (!lstua || lstua.length ===0) return [];
 return this.FilterActive(lstua, pIncludeAdmins);
 }

 private static IsActive(pUser: XUser): boolean {
 if (!pUser.IsActive) return false;
 return true;
 }

 private FilterActive(pUsers: XUser[], pIncludeAdmins: boolean): XUser[] {
 const rsl = [] as XUser[];
 for (const usr of pUsers) {
 if (!XUserService.IsActive(usr)) continue;
 if (!pIncludeAdmins && usr.IsAdmin) continue;
 rsl.push(usr);
 }
 return rsl;
 }
}
```

## Resumo

- `X` em classes, `XI` em interfaces, tudo em inglês.
- `PascalCase` em tipos, membros e parâmetros com prefixo `p`.
- Campos privados com `_` + `PascalCase`.
- Variáveis locais minúsculas, mnemônicas.
- Sem chaves para blocos de uma linha, sem comentários, evitar métodos anônimos.
- Priorizar performance, práticas modernas e clareza.

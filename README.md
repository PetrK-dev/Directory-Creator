# DirectoryCreator

**DirectoryCreator** je jednoduchá konzolová aplikace v jazyce C#, která umožňuje automaticky vytvářet struktury složek na základě předdefinovaných šablon. Uživatel může vybírat z různých šablon a aplikace vytvoří složky podle zvolené struktury. Ty jsou poté uloženy do složky sharedDisk, která zde představuje sdílený disk.

## Funkce

- Vytváření složek a podsložek podle definovaných šablon.
- Podpora více šablon uložených v souborech JSON.
- Možnost definovat vnořené struktury složek pomocí rekurze.
- Ověření, zda projekt se zadaným názvem již existuje, aby nedošlo k přepsání.

## Struktura projektu

- `Program.cs` - Hlavní soubor s kódem aplikace.
- `sharedDisk/` - Složka, do které se vytvářejí nové projekty a jejich struktury.
- `templates/` - Složka obsahující šablony ve formátu JSON.
  - `prace.json`
  - `prace_komplex.json`
  - `skola.json`
  - `volny_cas.json`

## Požadavky

- .NET Core SDK (verze 3.1 nebo novější)
- Knihovna **Newtonsoft.Json** pro práci s JSON (nainstalováno přes NuGet)

## Přidání nové šablony

1. **Vytvořte nový soubor JSON v adresáři `templates/`**, například `nova_sablona.json`.

2. **Definujte strukturu složek** v novém souboru. Formát by měl být následující:

   ```json
   {
     "folders": [
       { "name": "Složka1" },
       {
         "name": "Složka2",
         "folders": [
           { "name": "Podsložka1" },
           { "name": "Podsložka2" }
         ]
       }
     ]
   }

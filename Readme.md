# Warehouse modul: DaluxBuild

Dette modul henter data fra [Dalux Build](https://www.dalux.com/da/dalux-field) hver dag og gemmer dem i dit eget datavarehus på Azure.
Der er anvendt Dalux's web service. Der er nylig frigivet et REST API til erstatning for deres web service. Kontakt mig hvis der ønskes udarbejdet et modul som anvender deres nye API.

Modulet er bygget med [Bygdrift Warehouse](https://github.com/Bygdrift/Warehouse), der gør det muligt at vedhæfte flere moduler i det samme Azure miljø.
Modulet indsamler og vaske data fra alle slags tjenester, i en datalake og database.
Ved at gemme data til en MS SQL-database er det:
- let at hente data med Power BI, Excel og andre systemer
- let at kontrollere hvem der har adgang til hvad - faktisk kan det styres med AD, så du ikke behøver at håndtere legitimationsoplysninger
- Det er billigt

## Installation

Alle moduler kan installeres og faciliteres med ARM-skabeloner (Azure Resource Management): [Brug ARM-skabeloner til at opsætte og vedligeholde dette modul](https://github.com/HK-Byg/Warehouse.Modules.DaluxBuild/blob/master/Deploy).

## Kontakt

For information eller konsulenttid, kan du skrive til bygdrift@gmail.com.

## Database indhold

| TABLE_NAME        | COLUMN_NAME                   | DATA_TYPE      |
| :---------------- | :---------------------------- | :------------- |
| Projects          | ProjectID                     | int            |
| Projects          | Address                       | varchar        |
| Projects          | BoxType                       | varchar        |
| Projects          | Created                       | datetimeoffset |
| Projects          | Name                          | varchar        |
| Projects          | Number                        | varchar        |
| Projects          | * All meta data columns *     | varchar        |
| ProjectsUsers     | ProjectID                     | int            |
| ProjectsUsers     | UserID                        | varchar        |
| ProjectsUsers     | Email                         | varchar        |
| ProjectsUsers     | Firstname                     | varchar        |
| ProjectsUsers     | Lastname                      | varchar        |
| ProjectsUsers     | CompanyID                     | int            |
| ProjectsUsers     | CompanyName                   | varchar        |
| ProjectsUsers     | CompanyVAT                    | varchar        |
| ProjectsContracts | ProjectID                     | int            |
| ProjectsContracts | UserID                        | int            |
| ProjectsContracts | Email                         | int            |
| ProjectsContracts | Firstname                     | varchar        |
| ProjectsContracts | Lastname                      | varchar        |
| ProjectsContracts | CompanyID                     | varchar        |
| ProjectsContracts | CompanyName                   | varchar        |
| ProjectsContracts | CompanyVAT                    | varchar        |
| ProjectsApprovals | ApprovalID                    | int            |
| ProjectsApprovals | Number                        | varchar        |
| ProjectsApprovals | CreatedByUserMail             | varchar        |
| ProjectsApprovals | CreatedByUserID               | varchar        |
| ProjectsApprovals | CreatedByCompanyName          | varchar        |
| ProjectsApprovals | CreatedByCompanyId            | int            |
| ProjectsApprovals | Created                       | datetime       |
| ProjectsApprovals | InspectionType                | varchar        |
| ProjectsApprovals | IsDeleted                     | bit            |
| ProjectsApprovals | ProjectID                     | int            |
| ProjectsApprovals | ProjectName                   | varchar        |
| ProjectsApprovals | ProjectNumber                 | varchar        |
| ProjectsApprovals | RoleName                      | varchar        |
| ProjectsApprovals | RevisionList                  | varchar        |
| ProjectsApprovals | LocationList                  | varchar        |
| ProjectChecklists | BuildingObjectInfo            | varchar        |
| ProjectChecklists | ChecklistId                   | int            |
| ProjectChecklists | Closed                        | bit            |
| ProjectChecklists | CreatedByUserMail             | varchar        |
| ProjectChecklists | CreatedByUserID               | varchar        |
| ProjectChecklists | CreatedByCompanyName          | varchar        |
| ProjectChecklists | CreatedByCompanyID            | int            |
| ProjectChecklists | CreatedByDateTime             | datetime       |
| ProjectChecklists | IsDeleted                     | bit            |
| ProjectChecklists | LastModifiedByUserMail        | varchar        |
| ProjectChecklists | LastModifiedByUserID          | varchar        |
| ProjectChecklists | LastModifiedByCompanyName     | varchar        |
| ProjectChecklists | LastModifiedByCompanyID       | int            |
| ProjectChecklists | LastModifiedDateTime          | datetime       |
| ProjectChecklists | ProjectID                     | int            |
| ProjectChecklists | ProjectName                   | varchar        |
| ProjectChecklists | ProjectNumber                 | varchar        |
| ProjectChecklists | Safety                        | bit            |
| ProjectChecklists | LocationList                  | varchar        |
| ProjectChecklists | ExtensionsDataList            | varchar        |
| ProjectChecklists | ViewPointImageList            | varchar        |
| ProjectsCompanies | ProjectID                     | int            |
| ProjectsCompanies | CompanyID                     | int            |
| ProjectsCompanies | Address                       | varchar        |
| ProjectsCompanies | CountryCode                   | varchar        |
| ProjectsCompanies | Name                          | varchar        |
| ProjectsCompanies | VAT                           | varchar        |
| Log               | TimeStamp                     | datetimeoffset |
| Log               | Errors                        | int            |
| Log               | ErrorDetails                  | varchar        |

## Data lake indhold

In the data lake container med dette modulnavn er der to hovedmapper: `Raw` og `Refined`.

Mappestrukturen:
+ Raw
    - {yyyy the year}
        - {MM the month}
            - {dd the month}
                - projects.json
                - project `id`_approvals.json
                - project `id`_checklists.json
                - project `id`_companies.json
                - project `id`_contracts.json
                - project `id`_users.json
+ Refined
    - {yyyy the year}
        - {MM the month}
            - {dd the month}
                - Log.csv
                - Projects.csv
                - ProjectsApprovals.csv
                - ProjectsChecklists.csv
                - ProjectsCompanies.csv
                - ProjectsContracts.csv
                - ProjectsUsers.csv

# Licens

[MIT License](https://github.com/Bygdrift/Warehouse.Modules.Example/blob/master/License.md)
/*
========================================================
StudentDB - SQL skripta za kreiranje baze
Sadrži: bazu, tipove, tabele, relacije (FK/PK/CHECK/DEFAULT) i procedure
Izdvojeno iz originalne deployment skripte (uklonjen je deployment/DB-settings boilerplate)
========================================================
*/

CREATE DATABASE StudentDB;
GO

USE StudentDB;
GO

PRINT N'Creating User-Defined Table Type [dbo].[PreliminarnaRangListaTip]...';


GO
CREATE TYPE [dbo].[PreliminarnaRangListaTip] AS TABLE (
    [Pozicija]                        INT             NOT NULL,
    [StudentID]                       INT             NOT NULL,
    [Ime]                             NVARCHAR (100)  NOT NULL,
    [Prezime]                         NVARCHAR (100)  NOT NULL,
    [BrojIndeksa]                     NVARCHAR (50)   NOT NULL,
    [StudijskiProgram]                NVARCHAR (150)  NULL,
    [GodinaStudija]                   NVARCHAR (50)   NULL,
    [Prosek]                          DECIMAL (5, 2)  NOT NULL,
    [BezRoditelja]                    BIT             NOT NULL,
    [DokumentacijaPrihodaDostavljena] BIT             NOT NULL,
    [UkupnaPrimanjaDomacinstva]       DECIMAL (15, 2) NOT NULL,
    [UkupnoBodova]                    DECIMAL (10, 2) NOT NULL);


GO
PRINT N'Creating Table [dbo].[Korisnici]...';


GO
CREATE TABLE [dbo].[Korisnici] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [Ime]                   NVARCHAR (100) NOT NULL,
    [Prezime]               NVARCHAR (100) NOT NULL,
    [KorisnickoIme]         NVARCHAR (50)  NOT NULL,
    [LozinkaHash]           NVARCHAR (255) NOT NULL,
    [TipKorisnika]          TINYINT        NOT NULL,
    [Aktivan]               BIT            NOT NULL,
    [DatumKreiranja]        DATETIME2 (0)  NOT NULL,
    [DatumPoslednjePrijave] DATETIME2 (0)  NULL,
    CONSTRAINT [PK_Korisnici] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UQ_Korisnici_KorisnickoIme] UNIQUE NONCLUSTERED ([KorisnickoIme] ASC)
);


GO
PRINT N'Creating Table [dbo].[PreliminarnaRangLista]...';


GO
CREATE TABLE [dbo].[PreliminarnaRangLista] (
    [ID]                              INT             IDENTITY (1, 1) NOT NULL,
    [Pozicija]                        INT             NOT NULL,
    [StudentID]                       INT             NOT NULL,
    [Ime]                             NVARCHAR (100)  NOT NULL,
    [Prezime]                         NVARCHAR (100)  NOT NULL,
    [BrojIndeksa]                     NVARCHAR (50)   NOT NULL,
    [StudijskiProgram]                NVARCHAR (150)  NULL,
    [GodinaStudija]                   NVARCHAR (50)   NULL,
    [Prosek]                          DECIMAL (5, 2)  NOT NULL,
    [BezRoditelja]                    BIT             NOT NULL,
    [DokumentacijaPrihodaDostavljena] BIT             NOT NULL,
    [UkupnaPrimanjaDomacinstva]       DECIMAL (15, 2) NOT NULL,
    [UkupnoBodova]                    DECIMAL (10, 2) NOT NULL,
    [DatumObjave]                     DATETIME2 (7)   NOT NULL,
    [ObjavioKorisnikID]               INT             NOT NULL,
    CONSTRAINT [PK_PreliminarnaRangLista] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UQ_PreliminarnaRangLista_Pozicija] UNIQUE NONCLUSTERED ([Pozicija] ASC),
    CONSTRAINT [UQ_PreliminarnaRangLista_Student] UNIQUE NONCLUSTERED ([StudentID] ASC)
);


GO
PRINT N'Creating Table [dbo].[PrimedbeNaRangListu]...';


GO
CREATE TABLE [dbo].[PrimedbeNaRangListu] (
    [ID]                INT             IDENTITY (1, 1) NOT NULL,
    [Ime]               NVARCHAR (100)  NOT NULL,
    [Prezime]           NVARCHAR (100)  NOT NULL,
    [Email]             NVARCHAR (255)  NOT NULL,
    [Komentar]          NVARCHAR (2000) NOT NULL,
    [DatumPodnosenja]   DATETIME2 (7)   NOT NULL,
    [Obradjena]         BIT             NOT NULL,
    [DatumObrade]       DATETIME2 (7)   NULL,
    [ObradioKorisnikID] INT             NULL,
    CONSTRAINT [PK_PrimedbeNaRangListu] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating Table [dbo].[SkoringKonfiguracija]...';


GO
CREATE TABLE [dbo].[SkoringKonfiguracija] (
    [ID]                     INT             IDENTITY (1, 1) NOT NULL,
    [MinimalanProsek]        DECIMAL (5, 2)  NULL,
    [ProsekZa10Bodova]       DECIMAL (5, 2)  NULL,
    [ProsekZa15Bodova]       DECIMAL (5, 2)  NULL,
    [ProsekZa20Bodova]       DECIMAL (5, 2)  NULL,
    [PrimanjaZa10Bodova]     DECIMAL (15, 2) NULL,
    [PrimanjaZa1Bod]         DECIMAL (15, 2) NULL,
    [BodoviGodinaStudije1]   INT             NULL,
    [BodoviGodinaStudije2]   INT             NULL,
    [BodoviGodinaStudije3]   INT             NULL,
    [BodoviGodinaStudije4]   INT             NULL,
    [BodoviMaster]           INT             NULL,
    [BezRoditeljaMultiplier] DECIMAL (3, 2)  NULL,
    [DatumKreiranja]         DATETIME2 (7)   NULL,
    [DatumAzuriranja]        DATETIME2 (7)   NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating Table [dbo].[StudentDokumentacija]...';


GO
CREATE TABLE [dbo].[StudentDokumentacija] (
    [ID]                    INT            IDENTITY (1, 1) NOT NULL,
    [StudentID]             INT            NOT NULL,
    [SviDokumentiPodneseni] BIT            NOT NULL,
    [DatumPodnosenja]       DATETIME       NULL,
    [SviDokumentiProvereni] BIT            NOT NULL,
    [DatumProvere]          DATETIME       NULL,
    [NapomenaProvere]       NVARCHAR (500) NULL,
    [SpremnaZaUnos]         BIT            NOT NULL,
    [DatumKreiranja]        DATETIME       NOT NULL,
    [DatumAzuriranja]       DATETIME       NOT NULL,
    [ProverioKorisnikID]    INT            NULL,
    [DokumentacijaIspravna] BIT            NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating Index [dbo].[StudentDokumentacija].[UX_StudentDokumentacija_StudentID]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_StudentDokumentacija_StudentID]
    ON [dbo].[StudentDokumentacija]([StudentID] ASC);


GO
PRINT N'Creating Table [dbo].[StudentDokumenti]...';


GO
CREATE TABLE [dbo].[StudentDokumenti] (
    [ID]                 INT            IDENTITY (1, 1) NOT NULL,
    [StudentID]          INT            NOT NULL,
    [VrstaDokumentaID]   INT            NOT NULL,
    [Obavezan]           BIT            NOT NULL,
    [Podnet]             BIT            NOT NULL,
    [DatumPodnosenja]    DATETIME2 (7)  NULL,
    [Ispravan]           BIT            NULL,
    [DatumProvere]       DATETIME2 (7)  NULL,
    [ProverioKorisnikID] INT            NULL,
    [Napomena]           NVARCHAR (500) NULL,
    [DatumKreiranja]     DATETIME2 (7)  NOT NULL,
    [DatumAzuriranja]    DATETIME2 (7)  NOT NULL,
    CONSTRAINT [PK_StudentDokumenti] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UQ_StudentDokumenti_StudentVrsta] UNIQUE NONCLUSTERED ([StudentID] ASC, [VrstaDokumentaID] ASC)
);


GO
PRINT N'Creating Table [dbo].[Studenti]...';


GO
CREATE TABLE [dbo].[Studenti] (
    [ID]                              INT             IDENTITY (1, 1) NOT NULL,
    [Ime]                             NVARCHAR (100)  NOT NULL,
    [Prezime]                         NVARCHAR (100)  NOT NULL,
    [DatumRodjenja]                   DATE            NOT NULL,
    [Email]                           NVARCHAR (255)  NOT NULL,
    [Telefon]                         NVARCHAR (30)   NOT NULL,
    [BrojIndeksa]                     NVARCHAR (50)   NOT NULL,
    [StudijskiProgram]                NVARCHAR (150)  NOT NULL,
    [GodinaStudija]                   NVARCHAR (50)   NOT NULL,
    [BezRoditelja]                    BIT             NOT NULL,
    [Prosek]                          DECIMAL (5, 2)  NOT NULL,
    [DokumentacijaPrihodaDostavljena] BIT             NOT NULL,
    [UkupnaPrimanjaDomacinstva]       DECIMAL (15, 2) NOT NULL,
    [BrojClanovaPorodice]             INT             NOT NULL,
    [BodoviProsek]                    INT             NOT NULL,
    [BodoviPrimanja]                  INT             NOT NULL,
    [BodoviGodina]                    INT             NOT NULL,
    [UkupnoBodova]                    INT             NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
PRINT N'Creating Table [dbo].[VrsteDokumenata]...';


GO
CREATE TABLE [dbo].[VrsteDokumenata] (
    [ID]       INT            IDENTITY (1, 1) NOT NULL,
    [Naziv]    NVARCHAR (150) NOT NULL,
    [Obavezan] BIT            NOT NULL,
    [Aktivan]  BIT            NOT NULL,
    [Sifra]    NVARCHAR (50)  NULL,
    CONSTRAINT [PK_VrsteDokumenata] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UQ_VrsteDokumenata_Naziv] UNIQUE NONCLUSTERED ([Naziv] ASC)
);


GO
PRINT N'Creating Index [dbo].[VrsteDokumenata].[UX_VrsteDokumenata_Sifra]...';


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_VrsteDokumenata_Sifra]
    ON [dbo].[VrsteDokumenata]([Sifra] ASC) WHERE ([Sifra] IS NOT NULL);


GO
PRINT N'Creating Default Constraint [dbo].[DF_Korisnici_DatumKreiranja]...';


GO
ALTER TABLE [dbo].[Korisnici]
    ADD CONSTRAINT [DF_Korisnici_DatumKreiranja] DEFAULT (sysutcdatetime()) FOR [DatumKreiranja];


GO
PRINT N'Creating Default Constraint [dbo].[DF_Korisnici_Aktivan]...';


GO
ALTER TABLE [dbo].[Korisnici]
    ADD CONSTRAINT [DF_Korisnici_Aktivan] DEFAULT ((1)) FOR [Aktivan];


GO
PRINT N'Creating Default Constraint [dbo].[DF_PrimedbeNaRangListu_Obradjena]...';


GO
ALTER TABLE [dbo].[PrimedbeNaRangListu]
    ADD CONSTRAINT [DF_PrimedbeNaRangListu_Obradjena] DEFAULT ((0)) FOR [Obradjena];


GO
PRINT N'Creating Default Constraint [dbo].[DF_PrimedbeNaRangListu_Datum]...';


GO
ALTER TABLE [dbo].[PrimedbeNaRangListu]
    ADD CONSTRAINT [DF_PrimedbeNaRangListu_Datum] DEFAULT (sysutcdatetime()) FOR [DatumPodnosenja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((6)) FOR [BodoviGodinaStudije3];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((8.50)) FOR [ProsekZa10Bodova];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((1000)) FOR [PrimanjaZa10Bodova];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT (sysdatetime()) FOR [DatumKreiranja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((9.00)) FOR [ProsekZa15Bodova];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((8)) FOR [BodoviGodinaStudije4];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT (sysdatetime()) FOR [DatumAzuriranja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((11000)) FOR [PrimanjaZa1Bod];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((2)) FOR [BodoviGodinaStudije1];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((8.50)) FOR [MinimalanProsek];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((10)) FOR [BodoviMaster];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((9.50)) FOR [ProsekZa20Bodova];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((4)) FOR [BodoviGodinaStudije2];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[SkoringKonfiguracija]...';


GO
ALTER TABLE [dbo].[SkoringKonfiguracija]
    ADD DEFAULT ((1.80)) FOR [BezRoditeljaMultiplier];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[StudentDokumentacija]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD DEFAULT ((0)) FOR [SviDokumentiPodneseni];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[StudentDokumentacija]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD DEFAULT (getdate()) FOR [DatumAzuriranja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[StudentDokumentacija]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD DEFAULT ((0)) FOR [SviDokumentiProvereni];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[StudentDokumentacija]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD DEFAULT ((0)) FOR [SpremnaZaUnos];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[StudentDokumentacija]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD DEFAULT (getdate()) FOR [DatumKreiranja];


GO
PRINT N'Creating Default Constraint [dbo].[DF_StudentDokumenti_DatumAzuriranja]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [DF_StudentDokumenti_DatumAzuriranja] DEFAULT (sysutcdatetime()) FOR [DatumAzuriranja];


GO
PRINT N'Creating Default Constraint [dbo].[DF_StudentDokumenti_Podnet]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [DF_StudentDokumenti_Podnet] DEFAULT ((0)) FOR [Podnet];


GO
PRINT N'Creating Default Constraint [dbo].[DF_StudentDokumenti_DatumKreiranja]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [DF_StudentDokumenti_DatumKreiranja] DEFAULT (sysutcdatetime()) FOR [DatumKreiranja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [BodoviPrimanja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [BodoviProsek];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [UkupnoBodova];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [BodoviGodina];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [DokumentacijaPrihodaDostavljena];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [BezRoditelja];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((1)) FOR [BrojClanovaPorodice];


GO
PRINT N'Creating Default Constraint unnamed constraint on [dbo].[Studenti]...';


GO
ALTER TABLE [dbo].[Studenti]
    ADD DEFAULT ((0)) FOR [UkupnaPrimanjaDomacinstva];


GO
PRINT N'Creating Default Constraint [dbo].[DF_VrsteDokumenata_Obavezan]...';


GO
ALTER TABLE [dbo].[VrsteDokumenata]
    ADD CONSTRAINT [DF_VrsteDokumenata_Obavezan] DEFAULT ((1)) FOR [Obavezan];


GO
PRINT N'Creating Default Constraint [dbo].[DF_VrsteDokumenata_Aktivan]...';


GO
ALTER TABLE [dbo].[VrsteDokumenata]
    ADD CONSTRAINT [DF_VrsteDokumenata_Aktivan] DEFAULT ((1)) FOR [Aktivan];


GO
PRINT N'Creating Foreign Key [dbo].[FK_PreliminarnaRangLista_Korisnik]...';


GO
ALTER TABLE [dbo].[PreliminarnaRangLista]
    ADD CONSTRAINT [FK_PreliminarnaRangLista_Korisnik] FOREIGN KEY ([ObjavioKorisnikID]) REFERENCES [dbo].[Korisnici] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_PreliminarnaRangLista_Student]...';


GO
ALTER TABLE [dbo].[PreliminarnaRangLista]
    ADD CONSTRAINT [FK_PreliminarnaRangLista_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Studenti] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_PrimedbeNaRangListu_Korisnik]...';


GO
ALTER TABLE [dbo].[PrimedbeNaRangListu]
    ADD CONSTRAINT [FK_PrimedbeNaRangListu_Korisnik] FOREIGN KEY ([ObradioKorisnikID]) REFERENCES [dbo].[Korisnici] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_StudentDokumentacija_Student]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD CONSTRAINT [FK_StudentDokumentacija_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Studenti] ([ID]) ON DELETE CASCADE;


GO
PRINT N'Creating Foreign Key [dbo].[FK_StudentDokumentacija_ProverioKorisnik]...';


GO
ALTER TABLE [dbo].[StudentDokumentacija]
    ADD CONSTRAINT [FK_StudentDokumentacija_ProverioKorisnik] FOREIGN KEY ([ProverioKorisnikID]) REFERENCES [dbo].[Korisnici] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_StudentDokumenti_Student]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [FK_StudentDokumenti_Student] FOREIGN KEY ([StudentID]) REFERENCES [dbo].[Studenti] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_StudentDokumenti_Vrsta]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [FK_StudentDokumenti_Vrsta] FOREIGN KEY ([VrstaDokumentaID]) REFERENCES [dbo].[VrsteDokumenata] ([ID]);


GO
PRINT N'Creating Foreign Key [dbo].[FK_StudentDokumenti_Proverio]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [FK_StudentDokumenti_Proverio] FOREIGN KEY ([ProverioKorisnikID]) REFERENCES [dbo].[Korisnici] ([ID]);


GO
PRINT N'Creating Check Constraint [dbo].[CK_Korisnici_KorisnickoIme]...';


GO
ALTER TABLE [dbo].[Korisnici]
    ADD CONSTRAINT [CK_Korisnici_KorisnickoIme] CHECK (len(ltrim(rtrim([KorisnickoIme])))>=(3));


GO
PRINT N'Creating Check Constraint [dbo].[CK_Korisnici_TipKorisnika]...';


GO
ALTER TABLE [dbo].[Korisnici]
    ADD CONSTRAINT [CK_Korisnici_TipKorisnika] CHECK ([TipKorisnika]=(2) OR [TipKorisnika]=(1));


GO
PRINT N'Creating Check Constraint [dbo].[CK_StudentDokumenti_Podnosenje]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [CK_StudentDokumenti_Podnosenje] CHECK ([Podnet]=(0) AND [DatumPodnosenja] IS NULL OR [Podnet]=(1) AND [DatumPodnosenja] IS NOT NULL);


GO
PRINT N'Creating Check Constraint [dbo].[CK_StudentDokumenti_Napomena]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [CK_StudentDokumenti_Napomena] CHECK ([Ispravan] IS NULL OR [Ispravan]=(1) OR [Napomena] IS NOT NULL AND len(ltrim(rtrim([Napomena])))>(0));


GO
PRINT N'Creating Check Constraint [dbo].[CK_StudentDokumenti_Provera]...';


GO
ALTER TABLE [dbo].[StudentDokumenti]
    ADD CONSTRAINT [CK_StudentDokumenti_Provera] CHECK ([Ispravan] IS NULL AND [DatumProvere] IS NULL AND [ProverioKorisnikID] IS NULL OR [Ispravan] IS NOT NULL AND [Podnet]=(1) AND [DatumProvere] IS NOT NULL AND [ProverioKorisnikID] IS NOT NULL);


GO
PRINT N'Creating Procedure [dbo].[AzurirajDatumPoslednjePrijave]...';


GO
CREATE PROCEDURE dbo.AzurirajDatumPoslednjePrijave
    @ID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Korisnici
    SET DatumPoslednjePrijave = SYSUTCDATETIME()
    WHERE ID = @ID;
END;
GO
PRINT N'Creating Procedure [dbo].[DajDokumentacijuStudenata]...';


GO
CREATE   PROCEDURE dbo.DajDokumentacijuStudenata
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.ID,
        d.StudentID,
        d.SviDokumentiPodneseni,
        d.DatumPodnosenja,
        d.SviDokumentiProvereni,
        d.DatumProvere,
        d.DokumentacijaIspravna,
        d.NapomenaProvere,
        d.SpremnaZaUnos,
        d.DatumKreiranja,
        d.DatumAzuriranja,
        d.ProverioKorisnikID,

        s.Ime AS StudentIme,
        s.Prezime AS StudentPrezime,
        s.BrojIndeksa,

        k.Ime AS ProverioIme,
        k.Prezime AS ProverioPrezime,
        k.KorisnickoIme AS ProverioKorisnickoIme

    FROM dbo.StudentDokumentacija AS d

    INNER JOIN dbo.Studenti AS s
        ON s.ID = d.StudentID

    LEFT JOIN dbo.Korisnici AS k
        ON k.ID = d.ProverioKorisnikID

    ORDER BY
        CASE
            WHEN d.SviDokumentiPodneseni = 1
                 AND d.SviDokumentiProvereni = 0
                THEN 0

            WHEN d.SviDokumentiProvereni = 1
                 AND d.DokumentacijaIspravna = 0
                THEN 1

            WHEN d.SpremnaZaUnos = 1
                THEN 2

            ELSE 3
        END,
        s.Prezime,
        s.Ime;
END;
GO
PRINT N'Creating Procedure [dbo].[DajDokumenteStudenta]...';


GO
CREATE   PROCEDURE dbo.DajDokumenteStudenta
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
    BEGIN
        ;THROW 50061,
            N'Neispravan ID studenta.',
            1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Studenti
        WHERE ID = @StudentID
    )
    BEGIN
        ;THROW 50062,
            N'Student ne postoji.',
            1;
    END;

    SELECT
        d.ID,
        d.StudentID,
        d.VrstaDokumentaID,
        v.Naziv AS NazivDokumenta,
        d.Obavezan,
        d.Podnet,
        d.DatumPodnosenja,
        d.Ispravan,
        d.DatumProvere,
        d.ProverioKorisnikID,
        k.Ime + N' ' + k.Prezime AS ProverioImeIPrezime,
        d.Napomena,
        d.DatumKreiranja,
        d.DatumAzuriranja
    FROM dbo.StudentDokumenti AS d
    INNER JOIN dbo.VrsteDokumenata AS v
        ON v.ID = d.VrstaDokumentaID
    LEFT JOIN dbo.Korisnici AS k
        ON k.ID = d.ProverioKorisnikID
    WHERE d.StudentID = @StudentID
    ORDER BY
        d.Obavezan DESC,
        v.Naziv,
        d.ID;
END;
GO
PRINT N'Creating Procedure [dbo].[DajKorisnikaPoKorisnickomImenu]...';


GO
CREATE PROCEDURE dbo.DajKorisnikaPoKorisnickomImenu
    @KorisnickoIme NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID,
        Ime,
        Prezime,
        KorisnickoIme,
        LozinkaHash,
        TipKorisnika,
        Aktivan,
        DatumKreiranja,
        DatumPoslednjePrijave
    FROM dbo.Korisnici
    WHERE KorisnickoIme = LTRIM(RTRIM(@KorisnickoIme));
END;
GO
PRINT N'Creating Procedure [dbo].[DajPreliminarnuRangListu]...';


GO
CREATE   PROCEDURE dbo.DajPreliminarnuRangListu
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID,
        Pozicija,
        StudentID,
        Ime,
        Prezime,
        BrojIndeksa,
        StudijskiProgram,
        GodinaStudija,
        Prosek,
        BezRoditelja,
        DokumentacijaPrihodaDostavljena,
        UkupnaPrimanjaDomacinstva,
        UkupnoBodova,
        DatumObjave
    FROM dbo.PreliminarnaRangLista
    ORDER BY Pozicija;
END;
GO
PRINT N'Creating Procedure [dbo].[DajPrimedbeNaRangListu]...';


GO
CREATE   PROCEDURE dbo.DajPrimedbeNaRangListu
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        p.ID,
        p.Ime,
        p.Prezime,
        p.Email,
        p.Komentar,
        p.DatumPodnosenja,
        p.Obradjena,
        p.DatumObrade,
        p.ObradioKorisnikID,

        k.Ime AS ObradioIme,
        k.Prezime AS ObradioPrezime,
        k.KorisnickoIme AS ObradioKorisnickoIme

    FROM dbo.PrimedbeNaRangListu AS p

    LEFT JOIN dbo.Korisnici AS k
        ON k.ID = p.ObradioKorisnikID

    ORDER BY
        p.Obradjena,
        p.DatumPodnosenja DESC;
END;
GO
PRINT N'Creating Procedure [dbo].[DajStudentaPoID]...';


GO
CREATE   PROCEDURE dbo.DajStudentaPoID
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
    BEGIN
        ;THROW 50061,
            N'Neispravan ID studenta.',
            1;
    END;

    SELECT
        s.ID,
        s.Ime,
        s.Prezime,
        s.DatumRodjenja,
        s.Email,
        s.Telefon,
        s.BrojIndeksa,
        s.StudijskiProgram,
        s.GodinaStudija,
        s.BezRoditelja,
        s.Prosek,
        s.DokumentacijaPrihodaDostavljena,
        s.UkupnaPrimanjaDomacinstva,
        s.BrojClanovaPorodice,
        s.BodoviProsek,
        s.BodoviPrimanja,
        s.BodoviGodina,
        s.UkupnoBodova,

        CAST(
            CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.StudentDokumentacija AS d
                    WHERE d.StudentID = s.ID
                      AND d.SviDokumentiPodneseni = 1
                      AND d.SviDokumentiProvereni = 1
                      AND d.DokumentacijaIspravna = 1
                      AND d.SpremnaZaUnos = 1
                )
                THEN 1
                ELSE 0
            END
            AS BIT
        ) AS DokumentacijaSpremnaZaRangiranje

    FROM dbo.Studenti AS s
    WHERE s.ID = @StudentID;
END;
GO
PRINT N'Creating Procedure [dbo].[DajSveKorisnike]...';


GO
CREATE PROCEDURE dbo.DajSveKorisnike
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ID,
        Ime,
        Prezime,
        KorisnickoIme,
        TipKorisnika,
        Aktivan,
        DatumKreiranja,
        DatumPoslednjePrijave
    FROM dbo.Korisnici
    ORDER BY Ime, Prezime, KorisnickoIme;
END;
GO
PRINT N'Creating Procedure [dbo].[DajSveStudente]...';


GO
CREATE   PROCEDURE dbo.DajSveStudente
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.ID,
        s.Ime,
        s.Prezime,
        s.DatumRodjenja,
        s.Email,
        s.Telefon,
        s.BrojIndeksa,
        s.StudijskiProgram,
        s.GodinaStudija,
        s.BezRoditelja,
        s.Prosek,
        s.DokumentacijaPrihodaDostavljena,
        s.UkupnaPrimanjaDomacinstva,
        s.BrojClanovaPorodice,
        s.BodoviProsek,
        s.BodoviPrimanja,
        s.BodoviGodina,
        s.UkupnoBodova,

        CAST(
            CASE
                WHEN EXISTS
                (
                    SELECT 1
                    FROM dbo.StudentDokumentacija AS d
                    WHERE d.StudentID = s.ID
                      AND d.SviDokumentiPodneseni = 1
                      AND d.SviDokumentiProvereni = 1
                      AND d.DokumentacijaIspravna = 1
                      AND d.SpremnaZaUnos = 1
                )
                THEN 1
                ELSE 0
            END
            AS BIT
        ) AS DokumentacijaSpremnaZaRangiranje

    FROM dbo.Studenti AS s
    ORDER BY s.ID;
END;
GO
PRINT N'Creating Procedure [dbo].[DodajKorisnika]...';


GO
CREATE PROCEDURE dbo.DodajKorisnika
    @Ime NVARCHAR(100),
    @Prezime NVARCHAR(100),
    @KorisnickoIme NVARCHAR(50),
    @LozinkaHash NVARCHAR(255),
    @TipKorisnika TINYINT,
    @Aktivan BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SET @Ime = LTRIM(RTRIM(@Ime));
    SET @Prezime = LTRIM(RTRIM(@Prezime));
    SET @KorisnickoIme = LTRIM(RTRIM(@KorisnickoIme));

    IF @TipKorisnika NOT IN (1, 2)
        THROW 50001, N'Neispravan tip korisnika.', 1;

    IF EXISTS
    (
        SELECT 1
        FROM dbo.Korisnici
        WHERE KorisnickoIme = @KorisnickoIme
    )
        THROW 50002, N'Korisničko ime već postoji.', 1;

    INSERT INTO dbo.Korisnici
    (
        Ime,
        Prezime,
        KorisnickoIme,
        LozinkaHash,
        TipKorisnika,
        Aktivan
    )
    VALUES
    (
        @Ime,
        @Prezime,
        @KorisnickoIme,
        @LozinkaHash,
        @TipKorisnika,
        @Aktivan
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END;
GO
PRINT N'Creating Procedure [dbo].[DodajPrimedbuNaRangListu]...';


GO
CREATE   PROCEDURE dbo.DodajPrimedbuNaRangListu
    @Ime NVARCHAR(100),
    @Prezime NVARCHAR(100),
    @Email NVARCHAR(255),
    @Komentar NVARCHAR(2000)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.PreliminarnaRangLista
    )
    BEGIN
        ;THROW 50030,
            N'Preliminarna rang-lista nije objavljena.',
            1;
    END;

    SET @Ime =
        NULLIF(LTRIM(RTRIM(@Ime)), N'');

    SET @Prezime =
        NULLIF(LTRIM(RTRIM(@Prezime)), N'');

    SET @Email =
        NULLIF(LTRIM(RTRIM(@Email)), N'');

    SET @Komentar =
        NULLIF(LTRIM(RTRIM(@Komentar)), N'');

    IF @Ime IS NULL OR
       @Prezime IS NULL OR
       @Email IS NULL OR
       @Komentar IS NULL
    BEGIN
        ;THROW 50031,
            N'Sva polja za primedbu su obavezna.',
            1;
    END;

    IF LEN(@Komentar) < 10
    BEGIN
        ;THROW 50032,
            N'Komentar mora imati najmanje 10 karaktera.',
            1;
    END;

    INSERT INTO dbo.PrimedbeNaRangListu
    (
        Ime,
        Prezime,
        Email,
        Komentar
    )
    VALUES
    (
        @Ime,
        @Prezime,
        @Email,
        @Komentar
    );
END;
GO
PRINT N'Creating Procedure [dbo].[DodajStudenta]...';


GO
CREATE   PROCEDURE dbo.DodajStudenta
    @Ime NVARCHAR(100),
    @Prezime NVARCHAR(100),
    @DatumRodjenja DATE,
    @Email NVARCHAR(255),
    @Telefon NVARCHAR(30),
    @BrojIndeksa NVARCHAR(50),
    @StudijskiProgram NVARCHAR(150),
    @GodinaStudija NVARCHAR(50),
    @BezRoditelja BIT,
    @Prosek DECIMAL(5,2),
    @DokumentacijaPrihodaDostavljena BIT,
    @UkupnaPrimanjaDomacinstva DECIMAL(15,2),
    @BrojClanovaPorodice INT,
    @MaksimalanBrojPrijava INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @MaksimalanBrojPrijava IS NULL
       OR @MaksimalanBrojPrijava <= 0
    BEGIN
        ;THROW 50040,
            N'Maksimalan broj prijava nije ispravno podešen.',
            1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @TrenutniBrojPrijava INT;

        SELECT @TrenutniBrojPrijava = COUNT(*)
        FROM dbo.Studenti WITH (TABLOCKX, HOLDLOCK);

        IF @TrenutniBrojPrijava >= @MaksimalanBrojPrijava
        BEGIN
            ;THROW 50041,
                N'Dostignut je maksimalan broj prijava.',
                1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Studenti
            WHERE BrojIndeksa = @BrojIndeksa
        )
        BEGIN
            ;THROW 50042,
                N'Student sa unetim brojem indeksa već postoji.',
                1;
        END;

        -- Skup vrsta dokumenata za novu prijavu.
        DECLARE @VrsteZaPrijavu TABLE
        (
            ID INT PRIMARY KEY,
            Obavezan BIT NOT NULL
        );

        INSERT INTO @VrsteZaPrijavu (ID, Obavezan)
        SELECT ID, Obavezan
        FROM dbo.VrsteDokumenata WITH (HOLDLOCK)
        WHERE Aktivan = 1;

        IF NOT EXISTS
        (
            SELECT 1
            FROM @VrsteZaPrijavu
            WHERE Obavezan = 1
        )
        BEGIN
            ;THROW 50060,
                N'Nije podešena nijedna aktivna obavezna vrsta dokumenta.',
                1;
        END;

        INSERT INTO dbo.Studenti
        (
            Ime,
            Prezime,
            DatumRodjenja,
            Email,
            Telefon,
            BrojIndeksa,
            StudijskiProgram,
            GodinaStudija,
            BezRoditelja,
            Prosek,
            DokumentacijaPrihodaDostavljena,
            UkupnaPrimanjaDomacinstva,
            BrojClanovaPorodice
        )
        VALUES
        (
            @Ime,
            @Prezime,
            @DatumRodjenja,
            @Email,
            @Telefon,
            @BrojIndeksa,
            @StudijskiProgram,
            @GodinaStudija,
            @BezRoditelja,
            @Prosek,
            0,
            @UkupnaPrimanjaDomacinstva,
            @BrojClanovaPorodice
        );

        DECLARE @NoviStudentID INT =
            CAST(SCOPE_IDENTITY() AS INT);

        INSERT INTO dbo.StudentDokumentacija
        (
            StudentID,
            SviDokumentiPodneseni,
            DatumPodnosenja,
            SviDokumentiProvereni,
            DatumProvere,
            DokumentacijaIspravna,
            NapomenaProvere,
            SpremnaZaUnos,
            ProverioKorisnikID
        )
        VALUES
        (
            @NoviStudentID,
            0,
            NULL,
            0,
            NULL,
            NULL,
            NULL,
            0,
            NULL
        );

        INSERT INTO dbo.StudentDokumenti
        (
            StudentID,
            VrstaDokumentaID,
            Obavezan,
            Podnet,
            DatumPodnosenja,
            Ispravan,
            DatumProvere,
            ProverioKorisnikID,
            Napomena
        )
        SELECT
            @NoviStudentID,
            v.ID,
            v.Obavezan,
            0,
            NULL,
            NULL,
            NULL,
            NULL,
            NULL
        FROM @VrsteZaPrijavu AS v;

        COMMIT TRANSACTION;

        SELECT @NoviStudentID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[IzmeniStudenta]...';


GO
CREATE   PROCEDURE dbo.IzmeniStudenta
    @StudentID INT,
    @Ime NVARCHAR(100),
    @Prezime NVARCHAR(100),
    @DatumRodjenja DATE,
    @Email NVARCHAR(255),
    @Telefon NVARCHAR(30),
    @BrojIndeksa NVARCHAR(50),
    @StudijskiProgram NVARCHAR(150),
    @GodinaStudija NVARCHAR(50),
    @BezRoditelja BIT,
    @Prosek DECIMAL(5,2),
    @UkupnaPrimanjaDomacinstva DECIMAL(15,2),
    @BrojClanovaPorodice INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
    BEGIN
        ;THROW 50070, N'Neispravan ID studenta.', 1;
    END;

    IF NULLIF(LTRIM(RTRIM(@Ime)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@Prezime)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@Email)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@Telefon)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@BrojIndeksa)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@StudijskiProgram)), N'') IS NULL
       OR NULLIF(LTRIM(RTRIM(@GodinaStudija)), N'') IS NULL
    BEGIN
        ;THROW 50071, N'Popunite sva obavezna polja.', 1;
    END;

    IF @DatumRodjenja IS NULL
       OR @DatumRodjenja > CAST(SYSUTCDATETIME() AS DATE)
       OR @BezRoditelja IS NULL
       OR @Prosek IS NULL
       OR @Prosek < 6
       OR @Prosek > 10
       OR @UkupnaPrimanjaDomacinstva IS NULL
       OR @UkupnaPrimanjaDomacinstva < 0
       OR @UkupnaPrimanjaDomacinstva > 10000000
       OR @BrojClanovaPorodice IS NULL
       OR @BrojClanovaPorodice < 1
       OR @BrojClanovaPorodice > 20
    BEGIN
        ;THROW 50071, N'Podaci prijave nisu ispravni.', 1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Isti redosled zaključavanja kao pri dodavanju prijave.
        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Studenti WITH (TABLOCKX, HOLDLOCK)
            WHERE ID = @StudentID
        )
        BEGIN
            ;THROW 50072, N'Student nije pronađen.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.PreliminarnaRangLista WITH (UPDLOCK, HOLDLOCK)
            WHERE StudentID = @StudentID
        )
        BEGIN
            ;THROW 50073,
                N'Student je na objavljenoj preliminarnoj rang-listi. Izmena nije dozvoljena dok se objava ne zameni listom bez ovog studenta.',
                1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.Studenti
            WHERE BrojIndeksa = LTRIM(RTRIM(@BrojIndeksa))
              AND ID <> @StudentID
        )
        BEGIN
            ;THROW 50074,
                N'Drugi student sa unetim brojem indeksa već postoji.',
                1;
        END;

        UPDATE dbo.Studenti
        SET
            Ime = LTRIM(RTRIM(@Ime)),
            Prezime = LTRIM(RTRIM(@Prezime)),
            DatumRodjenja = @DatumRodjenja,
            Email = LTRIM(RTRIM(@Email)),
            Telefon = LTRIM(RTRIM(@Telefon)),
            BrojIndeksa = LTRIM(RTRIM(@BrojIndeksa)),
            StudijskiProgram = LTRIM(RTRIM(@StudijskiProgram)),
            GodinaStudija = LTRIM(RTRIM(@GodinaStudija)),
            BezRoditelja = @BezRoditelja,
            Prosek = @Prosek,
            UkupnaPrimanjaDomacinstva =
                @UkupnaPrimanjaDomacinstva,
            BrojClanovaPorodice = @BrojClanovaPorodice,
            BodoviProsek = 0,
            BodoviPrimanja = 0,
            BodoviGodina = 0,
            UkupnoBodova = 0
        WHERE ID = @StudentID;

        -- Dokumenti ostaju podneti, ali zahtevaju novu proveru.
        UPDATE dbo.StudentDokumentacija
        SET
            SviDokumentiProvereni = 0,
            DokumentacijaIspravna = NULL,
            DatumProvere = NULL,
            ProverioKorisnikID = NULL,
            NapomenaProvere = NULL,
            SpremnaZaUnos = 0,
            DatumAzuriranja = SYSUTCDATETIME()
        WHERE StudentID = @StudentID;

        UPDATE dbo.StudentDokumenti
        SET
            Ispravan = NULL,
            DatumProvere = NULL,
            ProverioKorisnikID = NULL,
            Napomena = NULL,
            DatumAzuriranja = SYSUTCDATETIME()
        WHERE StudentID = @StudentID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[ObjaviPreliminarnuRangListu]...';


GO
CREATE   PROCEDURE dbo.ObjaviPreliminarnuRangListu
    @ObjavioKorisnikID INT,
    @Stavke dbo.PreliminarnaRangListaTip READONLY
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Korisnici
        WHERE ID = @ObjavioKorisnikID
          AND Aktivan = 1
    )
    BEGIN
        ;THROW 50020,
            N'Korisnik koji objavljuje listu ne postoji ili nije aktivan.',
            1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM @Stavke
    )
    BEGIN
        ;THROW 50021,
            N'Nije moguće objaviti praznu rang-listu.',
            1;
    END;

    IF EXISTS
    (
        SELECT Pozicija
        FROM @Stavke
        GROUP BY Pozicija
        HAVING COUNT(*) > 1
    )
    BEGIN
        ;THROW 50022,
            N'Rang-lista sadrži duplirane pozicije.',
            1;
    END;

    IF EXISTS
    (
        SELECT StudentID
        FROM @Stavke
        GROUP BY StudentID
        HAVING COUNT(*) > 1
    )
    BEGIN
        ;THROW 50023,
            N'Rang-lista sadrži istog studenta više puta.',
            1;
    END;

    DECLARE @DatumObjave DATETIME2 =
        SYSUTCDATETIME();

    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM dbo.PreliminarnaRangLista;

        INSERT INTO dbo.PreliminarnaRangLista
        (
            Pozicija,
            StudentID,
            Ime,
            Prezime,
            BrojIndeksa,
            StudijskiProgram,
            GodinaStudija,
            Prosek,
            BezRoditelja,
            DokumentacijaPrihodaDostavljena,
            UkupnaPrimanjaDomacinstva,
            UkupnoBodova,
            DatumObjave,
            ObjavioKorisnikID
        )
        SELECT
            Pozicija,
            StudentID,
            Ime,
            Prezime,
            BrojIndeksa,
            StudijskiProgram,
            GodinaStudija,
            Prosek,
            BezRoditelja,
            DokumentacijaPrihodaDostavljena,
            UkupnaPrimanjaDomacinstva,
            UkupnoBodova,
            @DatumObjave,
            @ObjavioKorisnikID
        FROM @Stavke
        ORDER BY Pozicija;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[ObrisiStudenta]...';


GO
CREATE   PROCEDURE dbo.ObrisiStudenta
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
    BEGIN
        ;THROW 50070, N'Neispravan ID studenta.', 1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        IF NOT EXISTS
        (
            SELECT 1
            FROM dbo.Studenti WITH (TABLOCKX, HOLDLOCK)
            WHERE ID = @StudentID
        )
        BEGIN
            ;THROW 50072, N'Student nije pronađen.', 1;
        END;

        IF EXISTS
        (
            SELECT 1
            FROM dbo.PreliminarnaRangLista WITH (UPDLOCK, HOLDLOCK)
            WHERE StudentID = @StudentID
        )
        BEGIN
            ;THROW 50075,
                N'Student je na objavljenoj preliminarnoj rang-listi. Brisanje nije dozvoljeno dok se objava ne zameni listom bez ovog studenta.',
                1;
        END;

        DELETE FROM dbo.StudentDokumentacija
        WHERE StudentID = @StudentID;

        DELETE FROM dbo.StudentDokumenti
        WHERE StudentID = @StudentID;

        DELETE FROM dbo.Studenti
        WHERE ID = @StudentID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[OznaciDokumentacijuPodnetom]...';


GO
CREATE   PROCEDURE dbo.OznaciDokumentacijuPodnetom
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE dbo.StudentDokumentacija
        SET
            SviDokumentiPodneseni = 1,
            DatumPodnosenja = SYSUTCDATETIME(),
            DatumAzuriranja = SYSUTCDATETIME()
        WHERE StudentID = @StudentID
          AND SviDokumentiPodneseni = 0;

        IF @@ROWCOUNT = 0
        BEGIN
            THROW 50010,
                N'Dokumentacija ne postoji ili je već podneta.',
                1;
        END;

        UPDATE dbo.Studenti
        SET DokumentacijaPrihodaDostavljena = 1
        WHERE ID = @StudentID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[OznaciDokumentacijuSpremnom]...';


GO
CREATE   PROCEDURE dbo.OznaciDokumentacijuSpremnom
    @StudentID INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.StudentDokumentacija
    SET
        SpremnaZaUnos = 1,
        DatumAzuriranja = SYSUTCDATETIME()
    WHERE StudentID = @StudentID
      AND SviDokumentiPodneseni = 1
      AND SviDokumentiProvereni = 1
      AND SpremnaZaUnos = 0;

    IF @@ROWCOUNT = 0
    BEGIN
        THROW 50012,
            N'Dokumentacija mora biti proverena pre označavanja kao spremna.',
            1;
    END;
END;
GO
PRINT N'Creating Procedure [dbo].[OznaciPrimedbuObradjenom]...';


GO
CREATE   PROCEDURE dbo.OznaciPrimedbuObradjenom
    @PrimedbaID INT,
    @ObradioKorisnikID INT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Korisnici
        WHERE ID = @ObradioKorisnikID
          AND Aktivan = 1
    )
    BEGIN
        ;THROW 50033,
            N'Administrator ne postoji ili nije aktivan.',
            1;
    END;

    UPDATE dbo.PrimedbeNaRangListu
    SET
        Obradjena = 1,
        DatumObrade = SYSUTCDATETIME(),
        ObradioKorisnikID = @ObradioKorisnikID
    WHERE ID = @PrimedbaID
      AND Obradjena = 0;

    IF @@ROWCOUNT = 0
    BEGIN
        ;THROW 50034,
            N'Primedba ne postoji ili je već obrađena.',
            1;
    END;
END;
GO
PRINT N'Creating Procedure [dbo].[OznaciStudentDokumentPodnetim]...';


GO
CREATE   PROCEDURE dbo.OznaciStudentDokumentPodnetim
    @StudentID INT,
    @DokumentID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
       OR @DokumentID IS NULL OR @DokumentID <= 0
    BEGIN
        ;THROW 50063,
            N'Neispravan ID studenta ili dokumenta.',
            1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        -- Sve izmene dokumenata istog studenta treba
        -- da koriste ovo zaključavanje zbirnog zapisa.
        DECLARE @ZbirniID INT;

        SELECT @ZbirniID = ID
        FROM dbo.StudentDokumentacija WITH (UPDLOCK, HOLDLOCK)
        WHERE StudentID = @StudentID;

        IF @ZbirniID IS NULL
        BEGIN
            ;THROW 50064,
                N'Zbirni zapis dokumentacije studenta ne postoji.',
                1;
        END;

        DECLARE @Sada DATETIME2(7) = SYSUTCDATETIME();

        UPDATE dbo.StudentDokumenti
        SET
            Podnet = 1,
            DatumPodnosenja = @Sada,
            DatumAzuriranja = @Sada
        WHERE ID = @DokumentID
          AND StudentID = @StudentID
          AND Podnet = 0
          AND Ispravan IS NULL;

        IF @@ROWCOUNT = 0
        BEGIN
            ;THROW 50065,
                N'Dokument ne pripada studentu, ne postoji ili je već podnet.',
                1;
        END;

        DECLARE @BrojObaveznih INT;
        DECLARE @BrojPodnetih INT;
        DECLARE @PoslednjiDatumPodnosenja DATETIME2(7);

        SELECT
            @BrojObaveznih = COUNT(*),

            @BrojPodnetih = COALESCE(
                SUM(CASE WHEN Podnet = 1 THEN 1 ELSE 0 END),
                0),

            @PoslednjiDatumPodnosenja = MAX(DatumPodnosenja)
        FROM dbo.StudentDokumenti
        WHERE StudentID = @StudentID
          AND Obavezan = 1;

        DECLARE @SviPodneti BIT =
            CASE
                WHEN @BrojObaveznih > 0
                 AND @BrojPodnetih = @BrojObaveznih
                THEN 1
                ELSE 0
            END;

        UPDATE dbo.StudentDokumentacija
        SET
            SviDokumentiPodneseni = @SviPodneti,

            DatumPodnosenja =
                CASE
                    WHEN @SviPodneti = 1
                    THEN @PoslednjiDatumPodnosenja
                    ELSE NULL
                END,

            DatumAzuriranja = @Sada
        WHERE ID = @ZbirniID;

		UPDATE s
SET DokumentacijaPrihodaDostavljena =
    CASE
        WHEN EXISTS
        (
            SELECT 1
            FROM dbo.StudentDokumenti AS d
            INNER JOIN dbo.VrsteDokumenata AS v
                ON v.ID = d.VrstaDokumentaID
            WHERE d.StudentID = s.ID
              AND v.Sifra = N'POTVRDA_PRIHODA'
              AND d.Podnet = 1
        )
        THEN 1
        ELSE 0
    END
FROM dbo.Studenti AS s
WHERE s.ID = @StudentID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO
PRINT N'Creating Procedure [dbo].[ProveriDokumentacijuStudenta]...';


GO
CREATE   PROCEDURE dbo.ProveriDokumentacijuStudenta
    @StudentID INT,
    @DokumentacijaIspravna BIT,
    @Napomena NVARCHAR(500) = NULL,
    @ProverioKorisnikID INT
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Korisnici
        WHERE ID = @ProverioKorisnikID
          AND Aktivan = 1
    )
    BEGIN
        ;THROW 50013,
            N'Korisnik koji vrši proveru ne postoji ili nije aktivan.',
            1;
    END;

    IF @DokumentacijaIspravna IS NULL
    BEGIN
        ;THROW 50014,
            N'Izaberite da li je dokumentacija ispravna.',
            1;
    END;

    SET @Napomena =
        NULLIF(LTRIM(RTRIM(@Napomena)), N'');

    IF @DokumentacijaIspravna = 0
       AND @Napomena IS NULL
    BEGIN
        ;THROW 50014,
            N'Napomena je obavezna kada dokumentacija nije ispravna.',
            1;
    END;

    UPDATE dbo.StudentDokumentacija
    SET
        SviDokumentiProvereni = 1,
        DatumProvere = SYSUTCDATETIME(),
        DokumentacijaIspravna = @DokumentacijaIspravna,
        NapomenaProvere = @Napomena,
        SpremnaZaUnos = @DokumentacijaIspravna,
        ProverioKorisnikID = @ProverioKorisnikID,
        DatumAzuriranja = SYSUTCDATETIME()
    WHERE StudentID = @StudentID
      AND SviDokumentiPodneseni = 1
      AND
      (
          SviDokumentiProvereni = 0
          OR DokumentacijaIspravna = 0
      );

    IF @@ROWCOUNT = 0
    BEGIN
        ;THROW 50011,
            N'Dokumentacija ne postoji, nije podneta ili je već potvrđena kao ispravna.',
            1;
    END;
END;
GO
PRINT N'Creating Procedure [dbo].[ProveriStudentDokument]...';


GO
CREATE   PROCEDURE dbo.ProveriStudentDokument
    @StudentID INT,
    @DokumentID INT,
    @Ispravan BIT,
    @ProverioKorisnikID INT,
    @Napomena NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    IF @StudentID IS NULL OR @StudentID <= 0
       OR @DokumentID IS NULL OR @DokumentID <= 0
    BEGIN
        ;THROW 50063,
            N'Neispravan ID studenta ili dokumenta.',
            1;
    END;

    IF @Ispravan IS NULL
    BEGIN
        ;THROW 50066,
            N'Izaberite rezultat provere.',
            1;
    END;

    SET @Napomena =
        NULLIF(LTRIM(RTRIM(@Napomena)), N'');

    IF @Ispravan = 0 AND @Napomena IS NULL
    BEGIN
        ;THROW 50067,
            N'Napomena je obavezna kada dokument nije ispravan.',
            1;
    END;

    IF NOT EXISTS
    (
        SELECT 1
        FROM dbo.Korisnici
        WHERE ID = @ProverioKorisnikID
          AND Aktivan = 1
    )
    BEGIN
        ;THROW 50068,
            N'Korisnik koji vrši proveru ne postoji ili nije aktivan.',
            1;
    END;

    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @ZbirniID INT;

        SELECT @ZbirniID = ID
        FROM dbo.StudentDokumentacija WITH (UPDLOCK, HOLDLOCK)
        WHERE StudentID = @StudentID;

        IF @ZbirniID IS NULL
        BEGIN
            ;THROW 50064,
                N'Zbirni zapis dokumentacije studenta ne postoji.',
                1;
        END;

        DECLARE @Sada DATETIME2(7) = SYSUTCDATETIME();

        UPDATE dbo.StudentDokumenti
        SET
            Ispravan = @Ispravan,
            DatumProvere = @Sada,
            ProverioKorisnikID = @ProverioKorisnikID,
            Napomena = @Napomena,
            DatumAzuriranja = @Sada
        WHERE ID = @DokumentID
          AND StudentID = @StudentID
          AND Podnet = 1
          AND
          (
              Ispravan IS NULL
              OR Ispravan = 0
          );

        IF @@ROWCOUNT = 0
        BEGIN
            ;THROW 50069,
                N'Dokument ne postoji, ne pripada studentu, nije podnet ili je već potvrđen kao ispravan.',
                1;
        END;

        DECLARE @BrojObaveznih INT;
        DECLARE @BrojPodnetih INT;
        DECLARE @BrojProverenih INT;
        DECLARE @BrojIspravnih INT;
        DECLARE @BrojNeispravnih INT;
        DECLARE @DatumPodnosenja DATETIME2(7);

        SELECT
            @BrojObaveznih = COUNT(*),

            @BrojPodnetih = COALESCE(
                SUM(CASE WHEN Podnet = 1 THEN 1 ELSE 0 END), 0),

            @BrojProverenih = COALESCE(
                SUM(CASE WHEN Ispravan IS NOT NULL
                         THEN 1 ELSE 0 END), 0),

            @BrojIspravnih = COALESCE(
                SUM(CASE WHEN Ispravan = 1 THEN 1 ELSE 0 END), 0),

            @BrojNeispravnih = COALESCE(
                SUM(CASE WHEN Ispravan = 0 THEN 1 ELSE 0 END), 0),

            @DatumPodnosenja = MAX(DatumPodnosenja)
        FROM dbo.StudentDokumenti
        WHERE StudentID = @StudentID
          AND Obavezan = 1;

        DECLARE @SviPodneti BIT =
            CASE
                WHEN @BrojObaveznih > 0
                 AND @BrojPodnetih = @BrojObaveznih
                THEN 1 ELSE 0
            END;

        DECLARE @SviProvereni BIT =
            CASE
                WHEN @BrojObaveznih > 0
                 AND @BrojProverenih = @BrojObaveznih
                THEN 1 ELSE 0
            END;

        DECLARE @Spremna BIT =
            CASE
                WHEN @SviPodneti = 1
                 AND @BrojIspravnih = @BrojObaveznih
                THEN 1 ELSE 0
            END;

        DECLARE @ZbirnoIspravna BIT = NULL;

        IF @BrojNeispravnih > 0
            SET @ZbirnoIspravna = 0;
        ELSE IF @Spremna = 1
            SET @ZbirnoIspravna = 1;

        DECLARE @PoslednjaProvera DATETIME2(7);
        DECLARE @PoslednjiProverio INT;

        SELECT TOP (1)
            @PoslednjaProvera = DatumProvere,
            @PoslednjiProverio = ProverioKorisnikID
        FROM dbo.StudentDokumenti
        WHERE StudentID = @StudentID
          AND Obavezan = 1
          AND Ispravan IS NOT NULL
        ORDER BY DatumProvere DESC, ID DESC;

        UPDATE dbo.StudentDokumentacija
        SET
            SviDokumentiPodneseni = @SviPodneti,

            DatumPodnosenja =
                CASE WHEN @SviPodneti = 1
                     THEN @DatumPodnosenja ELSE NULL END,

            SviDokumentiProvereni = @SviProvereni,
            DokumentacijaIspravna = @ZbirnoIspravna,
            SpremnaZaUnos = @Spremna,

            DatumProvere = @PoslednjaProvera,
            ProverioKorisnikID = @PoslednjiProverio,

            NapomenaProvere =
                CASE
                    WHEN @BrojNeispravnih > 0
                        THEN N'Postoje neispravni obavezni dokumenti. Pogledajte detalje prijave.'
                    WHEN @Spremna = 1
                        THEN N'Svi obavezni dokumenti su ispravni.'
                    ELSE N'Provera obaveznih dokumenata nije završena.'
                END,

            DatumAzuriranja = @Sada
        WHERE ID = @ZbirniID;

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
        BEGIN
            ROLLBACK TRANSACTION;
        END;

        THROW;
    END CATCH;
END;
GO

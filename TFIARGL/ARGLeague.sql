USE [master]
GO
/****** Object:  Database [ARGLeague]    Script Date: 29/03/2018 11:00:13 ******/
CREATE DATABASE [ARGLeague]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Saitama', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Saitama.mdf' , SIZE = 4288KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Saitama_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\Saitama_log.ldf' , SIZE = 3200KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ARGLeague] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ARGLeague].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ARGLeague] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ARGLeague] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ARGLeague] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ARGLeague] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ARGLeague] SET ARITHABORT OFF 
GO
ALTER DATABASE [ARGLeague] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ARGLeague] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ARGLeague] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ARGLeague] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ARGLeague] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ARGLeague] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ARGLeague] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ARGLeague] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ARGLeague] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ARGLeague] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ARGLeague] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ARGLeague] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ARGLeague] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ARGLeague] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ARGLeague] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ARGLeague] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ARGLeague] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ARGLeague] SET RECOVERY FULL 
GO
ALTER DATABASE [ARGLeague] SET  MULTI_USER 
GO
ALTER DATABASE [ARGLeague] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ARGLeague] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ARGLeague] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ARGLeague] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ARGLeague] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ARGLeague]
GO
/****** Object:  Table [dbo].[BitacoraAuditoria]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BitacoraAuditoria](
	[Valor_Anterior] [varchar](250) NOT NULL,
	[Valor_Posterior] [varchar](250) NOT NULL,
	[ID_Bitacora_Auditoria] [int] IDENTITY(1,1) NOT NULL,
	[Detalle] [varchar](500) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[ID_Usuario] [bigint] NOT NULL,
	[IP_Usuario] [nvarchar](50) NOT NULL,
	[WebBrowser] [varchar](250) NOT NULL,
	[Tipo_Bitacora] [varchar](50) NOT NULL,
	[DVH] [nvarchar](50) NULL,
 CONSTRAINT [PK_Bitacora_Auditoria] PRIMARY KEY CLUSTERED 
(
	[ID_Bitacora_Auditoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BitacoraErrores]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BitacoraErrores](
	[URL] [varchar](250) NOT NULL,
	[StackTrace] [varchar](250) NOT NULL,
	[Exception] [varchar](250) NOT NULL,
	[ID_Bitacora_Errores] [int] IDENTITY(1,1) NOT NULL,
	[Detalle] [varchar](500) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[ID_Usuario] [bigint] NOT NULL,
	[IP_Usuario] [nvarchar](50) NOT NULL,
	[WebBrowser] [varchar](250) NOT NULL,
	[Tipo_Bitacora] [varchar](50) NOT NULL,
	[DVH] [nvarchar](50) NULL,
 CONSTRAINT [PK_BitacoraErrores] PRIMARY KEY CLUSTERED 
(
	[ID_Bitacora_Errores] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Control]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Control](
	[ID_Control] [int] NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Tipo] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Control] PRIMARY KEY CLUSTERED 
(
	[ID_Control] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Detalle_Factura]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Detalle_Factura](
	[ID_Detalle_Factura] [bigint] NOT NULL,
	[ID_Factura] [bigint] NOT NULL,
	[ID_Usuario] [bigint] NULL,
	[Monto] [float] NULL,
 CONSTRAINT [PK_Detalle_Factura] PRIMARY KEY CLUSTERED 
(
	[ID_Detalle_Factura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Digito_Verificador_Vertical]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Digito_Verificador_Vertical](
	[Nombre_Tabla] [varchar](50) NOT NULL,
	[Digito] [nvarchar](50) NULL,
 CONSTRAINT [PK_Digito_Vertical] PRIMARY KEY CLUSTERED 
(
	[Nombre_Tabla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Equipo]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Equipo](
	[ID_Equipo] [bigint] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Fecha_Creacion] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[ID_Game] [int] NULL,
	[Historia] [varchar](500) NULL,
 CONSTRAINT [PK_Equipo] PRIMARY KEY CLUSTERED 
(
	[ID_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Equipo_Historico]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Equipo_Historico](
	[ID_Equipo_Hist] [bigint] NOT NULL,
	[Nombre] [nvarchar](50) NULL,
	[Historia] [varchar](50) NULL,
	[ID_Equipo] [bigint] NULL,
 CONSTRAINT [PK_Equipo_Historico] PRIMARY KEY CLUSTERED 
(
	[ID_Equipo_Hist] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Estadistica]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Estadistica](
	[ID_Jugador] [bigint] NOT NULL,
	[ID_Partida] [bigint] NOT NULL,
	[ID_Equipo] [bigint] NOT NULL,
	[ID_Tipo_Estadistica] [int] NOT NULL,
	[Valor_Estadistica] [decimal](10, 2) NOT NULL,
 CONSTRAINT [PK_Estadistica] PRIMARY KEY CLUSTERED 
(
	[ID_Jugador] ASC,
	[ID_Partida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Factura]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Factura](
	[ID_Factura] [bigint] NOT NULL,
	[ID_Torneo] [bigint] NULL,
	[ID_Usuario] [bigint] NULL,
	[ID_Tipo_Pago] [int] NULL,
	[Monto_Total] [float] NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_Factura] PRIMARY KEY CLUSTERED 
(
	[ID_Factura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Fases]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Fases](
	[ID_Fase] [int] NOT NULL,
	[Descripcion] [varchar](75) NULL,
 CONSTRAINT [PK_Fases] PRIMARY KEY CLUSTERED 
(
	[ID_Fase] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Game]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Game](
	[ID_Game] [int] NOT NULL,
	[Nombre] [varchar](50) NULL,
	[ID_Tipo_Game] [int] NULL,
	[Reglas] [varchar](500) NULL,
	[Cantidad_Max_Jugadores] [int] NULL,
	[Descripcion] [varchar](250) NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[ID_Game] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Game_Estadistica]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game_Estadistica](
	[ID_Game] [int] NOT NULL,
	[ID_Tipo_Estadistica] [int] NOT NULL,
 CONSTRAINT [PK_Game_Estadistica] PRIMARY KEY CLUSTERED 
(
	[ID_Game] ASC,
	[ID_Tipo_Estadistica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Idioma]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Idioma](
	[ID_Idioma] [int] NOT NULL,
	[Nombre] [varchar](25) NOT NULL,
	[Editable] [bit] NOT NULL,
	[Cultura] [varchar](50) NOT NULL,
	[BL] [bit] NOT NULL,
 CONSTRAINT [PK_Idioma] PRIMARY KEY CLUSTERED 
(
	[ID_Idioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Jugador]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jugador](
	[ID_Jugador] [bigint] NOT NULL,
	[ID_Usuario] [bigint] NULL,
	[Nickname] [nvarchar](50) NULL,
	[ID_Game] [int] NULL,
	[Game_Tag] [nvarchar](100) NULL,
	[ID_Rol_Jugador] [int] NULL,
 CONSTRAINT [PK_Jugador] PRIMARY KEY CLUSTERED 
(
	[ID_Jugador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Jugador_Equipo]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jugador_Equipo](
	[ID_Jug_Equipo] [bigint] NOT NULL,
	[ID_Jugador] [bigint] NOT NULL,
	[ID_Equipo] [bigint] NOT NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
 CONSTRAINT [PK_Jugador_Equipo] PRIMARY KEY CLUSTERED 
(
	[ID_Jug_Equipo] ASC,
	[ID_Jugador] ASC,
	[ID_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Jugador_Historico]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Jugador_Historico](
	[ID_Jug_Hist] [bigint] NOT NULL,
	[ID_Jugador] [bigint] NULL,
	[Nickname] [nvarchar](50) NULL,
	[ID_Rol_Jugador] [int] NULL,
 CONSTRAINT [PK_Jugador_Historico] PRIMARY KEY CLUSTERED 
(
	[ID_Jug_Hist] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Partida]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Partida](
	[ID_Partida] [bigint] NOT NULL,
	[ID_Torneo] [bigint] NULL,
	[ID_Equipo_Local] [bigint] NULL,
	[ID_Equipo_Visitante] [bigint] NULL,
	[Ganador_Local] [bit] NULL,
	[Resultado] [varchar](100) NULL,
	[FechaHora] [datetime] NULL,
	[ID_Fase] [int] NULL,
 CONSTRAINT [PK_Partida] PRIMARY KEY CLUSTERED 
(
	[ID_Partida] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permiso]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Permiso](
	[ID_Rol] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](50) NULL,
	[url] [varchar](50) NULL,
	[esAccion] [bit] NOT NULL,
 CONSTRAINT [PK_Rol] PRIMARY KEY CLUSTERED 
(
	[ID_Rol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Permiso_Permiso]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permiso_Permiso](
	[ID_Rol] [int] NOT NULL,
	[ID_Permiso] [int] NOT NULL,
 CONSTRAINT [PK_Permiso] PRIMARY KEY CLUSTERED 
(
	[ID_Rol] ASC,
	[ID_Permiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Posicion]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Posicion](
	[ID_Posicion] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_Posicion] PRIMARY KEY CLUSTERED 
(
	[ID_Posicion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Premios]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Premios](
	[ID_Premio] [bigint] NOT NULL,
	[ID_Torneo] [bigint] NULL,
	[Nombre] [varchar](50) NULL,
	[ID_Posicion] [int] NULL,
	[Descripcion] [varchar](100) NULL,
	[Valor] [float] NULL,
 CONSTRAINT [PK_Premios] PRIMARY KEY CLUSTERED 
(
	[ID_Premio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Rol_Jugador]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Rol_Jugador](
	[ID_Rol_Jugador] [int] NOT NULL,
	[ID_Game] [int] NULL,
	[Nombre] [varchar](50) NULL,
	[ID_Tipo_Rol_Jugador] [int] NULL,
 CONSTRAINT [PK_Rol_Jugador] PRIMARY KEY CLUSTERED 
(
	[ID_Rol_Jugador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Solicitud_Invitacion]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Solicitud_Invitacion](
	[ID_Solicitud] [bigint] NOT NULL,
	[ID_Equipo] [bigint] NULL,
	[ID_Jugador] [bigint] NULL,
	[Mensaje] [nvarchar](50) NULL,
	[Jug_a_Equipo] [bit] NULL,
	[Fecha] [datetime] NULL,
 CONSTRAINT [PK_Solicitud_Invitacion] PRIMARY KEY CLUSTERED 
(
	[ID_Solicitud] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sponsor]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Sponsor](
	[ID_Sponsor] [int] NOT NULL,
	[Nombre] [varchar](100) NULL,
	[Cuil] [varchar](15) NULL,
	[Correo] [nvarchar](100) NULL,
 CONSTRAINT [PK_Sponsor] PRIMARY KEY CLUSTERED 
(
	[ID_Sponsor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tipo_Estadistica]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tipo_Estadistica](
	[ID_Tipo_Estadistica] [int] NOT NULL,
	[ID_Tipo_Rol_Jugador] [int] NOT NULL,
	[Descripcion] [varchar](100) NULL,
	[Valor_Base] [int] NULL,
 CONSTRAINT [PK_Tipo_Estadistica] PRIMARY KEY CLUSTERED 
(
	[ID_Tipo_Estadistica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tipo_Game]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tipo_Game](
	[ID_Tipo_Game] [int] NOT NULL,
	[Descripcion] [varchar](100) NULL,
 CONSTRAINT [PK_Tipo_Game] PRIMARY KEY CLUSTERED 
(
	[ID_Tipo_Game] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tipo_Pago]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tipo_Pago](
	[ID_Tipo_Pago] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_Tipo_Pago] PRIMARY KEY CLUSTERED 
(
	[ID_Tipo_Pago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tipo_Rol_Jugador]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tipo_Rol_Jugador](
	[ID_Tipo_Rol_Jugador] [int] NOT NULL,
	[Descripcion] [varchar](50) NULL,
 CONSTRAINT [PK_Tipo_Rol_Jugador] PRIMARY KEY CLUSTERED 
(
	[ID_Tipo_Rol_Jugador] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Torneo]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Torneo](
	[ID_Torneo] [bigint] NOT NULL,
	[Fecha_Inicio] [datetime] NULL,
	[Fecha_Fin] [datetime] NULL,
	[Nombre] [varchar](50) NULL,
	[Lugar_Final] [varchar](50) NULL,
	[ID_Sponsor] [int] NULL,
	[Precio_Inscrpcion] [float] NULL,
	[Fecha_Inicio_Inscripcion] [datetime] NULL,
	[Fecha_Fin_Inscripcion] [datetime] NULL,
 CONSTRAINT [PK_Torneo] PRIMARY KEY CLUSTERED 
(
	[ID_Torneo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Torneo_Equipo]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Torneo_Equipo](
	[ID_Torneo] [bigint] NOT NULL,
	[ID_Equipo] [bigint] NOT NULL,
	[ID_Posicion] [int] NULL,
	[ID_Premio] [bigint] NULL,
 CONSTRAINT [PK_Torneo_Equipo] PRIMARY KEY CLUSTERED 
(
	[ID_Torneo] ASC,
	[ID_Equipo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Traduccion]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Traduccion](
	[ID_Control] [int] NOT NULL,
	[ID_Idioma] [int] NOT NULL,
	[Palabra] [nvarchar](500) NULL,
 CONSTRAINT [PK_Traduccion] PRIMARY KEY CLUSTERED 
(
	[ID_Control] ASC,
	[ID_Idioma] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 29/03/2018 11:00:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Usuario](
	[ID_usuario] [bigint] IDENTITY(1,1) NOT NULL,
	[NombreUsuario] [varchar](150) NOT NULL,
	[Apellido] [nvarchar](75) NOT NULL,
	[Nombre] [nvarchar](75) NOT NULL,
	[Fecha_Alta] [datetime2](0) NOT NULL,
	[Salt] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](max) NOT NULL,
	[Intentos] [int] NOT NULL,
	[Bloqueo] [bit] NOT NULL,
	[ID_Perfil] [int] NOT NULL,
	[ID_Idioma] [int] NOT NULL,
	[Empleado] [bit] NOT NULL,
	[BL] [bit] NOT NULL,
	[DVH] [nvarchar](50) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[ID_usuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Digito_Verificador_Vertical] ([Nombre_Tabla], [Digito]) VALUES (N'Usuario', N'4bj9paX30fo/pme8O8KQ/KSmyDE=')
INSERT [dbo].[Idioma] ([ID_Idioma], [Nombre], [Editable], [Cultura], [BL]) VALUES (1, N'Español', 0, N'es-AR', 0)
SET IDENTITY_INSERT [dbo].[Permiso] ON 

INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (-1, N'PerfilEliminado', NULL, 0)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (0, N'Cliente', NULL, 0)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (1, N'CopiaSeguridad', N'/backup.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (2, N'Restauracion', N'/restore.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (3, N'BitacoraAuditoria', N'/BitacoraAuditoria.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (4, N'AgregarUsuario', N'/AgregarUsuario.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (5, N'Carrito', N'/Orders.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (6, N'MisCompras', N'/MyOrders.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (7, N'ListaProductos', N'/ProductList.aspx', 1)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (8, N'Administrador', NULL, 0)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (9, N'Usuario', NULL, 0)
INSERT [dbo].[Permiso] ([ID_Rol], [Nombre], [url], [esAccion]) VALUES (10, N'AgregarPerfil', N'/AgregarPerfil.aspx', 1)
SET IDENTITY_INSERT [dbo].[Permiso] OFF
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (8, 1)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (8, 2)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (8, 3)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (8, 4)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (8, 10)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (9, 5)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (9, 6)
INSERT [dbo].[Permiso_Permiso] ([ID_Rol], [ID_Permiso]) VALUES (9, 7)
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([ID_usuario], [NombreUsuario], [Apellido], [Nombre], [Fecha_Alta], [Salt], [Password], [Intentos], [Bloqueo], [ID_Perfil], [ID_Idioma], [Empleado], [BL], [DVH]) VALUES (8, N'admin', N'Tassara', N'Marcos', CAST(N'2018-03-28 17:19:35.0000000' AS DateTime2), N'263316379', N'hjIo4L2itWJg7RIgM10J8Y2+LOo=', 0, 0, 9, 1, 1, 0, N'UqpSs0nNNRgUXvfXjak26mu2UcQ=')
INSERT [dbo].[Usuario] ([ID_usuario], [NombreUsuario], [Apellido], [Nombre], [Fecha_Alta], [Salt], [Password], [Intentos], [Bloqueo], [ID_Perfil], [ID_Idioma], [Empleado], [BL], [DVH]) VALUES (9, N'marcos.tassara2@gmail.com', N'Tassara', N'Marcos', CAST(N'2018-03-28 18:28:50.0000000' AS DateTime2), N'1325751307', N'JvchKnfSpp0LdVNL1Fy41uYIYl8=', 0, 0, 0, 1, 0, 0, N'8kA66qnl5q/nQMRQgyFMXkeu/A0=')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
ALTER TABLE [dbo].[BitacoraAuditoria]  WITH CHECK ADD  CONSTRAINT [FK_BitacoraAuditoria_Usuario] FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuario] ([ID_usuario])
GO
ALTER TABLE [dbo].[BitacoraAuditoria] CHECK CONSTRAINT [FK_BitacoraAuditoria_Usuario]
GO
ALTER TABLE [dbo].[BitacoraErrores]  WITH CHECK ADD  CONSTRAINT [FK_BitacoraErrores_Usuario] FOREIGN KEY([ID_Usuario])
REFERENCES [dbo].[Usuario] ([ID_usuario])
GO
ALTER TABLE [dbo].[BitacoraErrores] CHECK CONSTRAINT [FK_BitacoraErrores_Usuario]
GO
ALTER TABLE [dbo].[Permiso_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_Permiso_Permiso] FOREIGN KEY([ID_Rol])
REFERENCES [dbo].[Permiso] ([ID_Rol])
GO
ALTER TABLE [dbo].[Permiso_Permiso] CHECK CONSTRAINT [FK_Permiso_Permiso_Permiso]
GO
ALTER TABLE [dbo].[Permiso_Permiso]  WITH CHECK ADD  CONSTRAINT [FK_Permiso_Permiso_Permiso1] FOREIGN KEY([ID_Permiso])
REFERENCES [dbo].[Permiso] ([ID_Rol])
GO
ALTER TABLE [dbo].[Permiso_Permiso] CHECK CONSTRAINT [FK_Permiso_Permiso_Permiso1]
GO
ALTER TABLE [dbo].[Traduccion]  WITH CHECK ADD  CONSTRAINT [FK_Traduccion_Control] FOREIGN KEY([ID_Control])
REFERENCES [dbo].[Control] ([ID_Control])
GO
ALTER TABLE [dbo].[Traduccion] CHECK CONSTRAINT [FK_Traduccion_Control]
GO
ALTER TABLE [dbo].[Traduccion]  WITH CHECK ADD  CONSTRAINT [FK_Traduccion_Idioma] FOREIGN KEY([ID_Idioma])
REFERENCES [dbo].[Idioma] ([ID_Idioma])
GO
ALTER TABLE [dbo].[Traduccion] CHECK CONSTRAINT [FK_Traduccion_Idioma]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Cliente_Rol] FOREIGN KEY([ID_Perfil])
REFERENCES [dbo].[Permiso] ([ID_Rol])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Cliente_Rol]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_Idioma] FOREIGN KEY([ID_Idioma])
REFERENCES [dbo].[Idioma] ([ID_Idioma])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_Idioma]
GO
USE [master]
GO
ALTER DATABASE [ARGLeague] SET  READ_WRITE 
GO

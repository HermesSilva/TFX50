# Instalação da Extensão DASE4VS

Um guia interativo para instalar a extensão no Visual Studio 2022 via linha de comando, para qualquer edição.

---

## Visão Geral

Este guia transforma o processo de instalação em uma série de passos simples e claros.  
O objetivo é fornecer todas as informações e comandos necessários de forma objetiva, permitindo copiar comandos essenciais para evitar erros.  
Funciona para as edições **Community**, **Professional** e **Enterprise** do Visual Studio 2022.

---

## 0. Pré-requisitos

Antes de começar, certifique-se de que você tem:

- ✔ **Visual Studio 2022:** Versão 17.14.11 ou superior, em qualquer edição.
- ✔ **Arquivo da Extensão:** `DASE4VS.vsix` salvo no seu computador.
- ✔ **Arquivo de Instalaçõ do DASE**  [📥 Baixar DASE4VS.vsix](https://github.com/HermesSilva/TFX50/tree/main/DASE/DASE4VS.vsix)

---

## 1. Localizar o Instalador

Encontre o `VSIXInstaller.exe`, usado para instalar extensões.

Caminho genérico (substitua `<Edição>` pela sua versão do VS):

```text
C:\Program Files\Microsoft Visual Studio\2022\<Edição>\Common7\IDE\VSIXInstaller.exe
```

**Exemplo:** Se for a edição *Professional*:  
`C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\VSIXInstaller.exe`

---

## 2. Abrir o Prompt de Comando

A instalação requer permissões de administrador:

1. Acesse o menu **Iniciar** do Windows.
2. Digite `cmd` ou `PowerShell`.
3. Clique com o botão direito e selecione **"Executar como administrador"**.

---

## 3. Executar o Comando de Instalação

No prompt de comando, execute:

```text
"C:\Program Files\Microsoft Visual Studio\2022\<Edição>\Common7\IDE\VSIXInstaller.exe" /i "C:\Caminho\para\seu\arquivo\DASE4VS.vsix"
```

**Importante:**  
- Substitua `<Edição>` por *Community*, *Professional* ou *Enterprise*.  
- Substitua `C:\Caminho\para\seu\arquivo\` pelo local real do arquivo.

---

## ✔ Conclusão

Após executar o comando, o instalador do Visual Studio abrirá para confirmar.  
Concluída a instalação, verifique em **Extensões > Gerenciar Extensões** no Visual Studio.

---

*Guia genérico para instalação via linha de comando.*

# GitHub Setup Guide

This guide will help you push the Renault Smart Center project to GitHub.

## 📋 Prerequisites

1. **Git installed** on your machine
   - Download: https://git-scm.com/downloads
   - Verify: `git --version`

2. **GitHub account** 
   - Create at: https://github.com/signup

## 🚀 Step-by-Step Instructions

### Step 1: Create a New Repository on GitHub

1. Go to https://github.com/new
2. **Repository name**: `Renault-Smart-Center` (or your preferred name)
3. **Description**: "Enterprise ERP System for Renault Authorized Service Centers"
4. **Visibility**: Choose Public or Private
5. **DO NOT** initialize with README, .gitignore, or license (we already have these)
6. Click **"Create repository"**

### Step 2: Initialize Git in Your Project

Open PowerShell or Command Prompt in your project folder and run:

```powershell
# Navigate to project folder (if not already there)
cd "D:\Personal\Renault Smart Center"

# Initialize git repository
git init

# Add all files
git add .

# Create initial commit
git commit -m "Initial commit: Renault Smart Center ERP System"

# Add remote repository (replace YOUR_USERNAME with your GitHub username)
git remote add origin https://github.com/YOUR_USERNAME/Renault-Smart-Center.git

# Push to GitHub
git branch -M main
git push -u origin main
```

### Step 3: If Repository Already Exists on GitHub

If you've already created a repository with some files, you might need to pull first:

```powershell
# Pull and merge (if needed)
git pull origin main --allow-unrelated-histories

# Then push
git push -u origin main
```

## 🔐 Authentication

### Option 1: Personal Access Token (Recommended)

1. Go to GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Click "Generate new token (classic)"
3. Give it a name and select scopes: `repo` (full control)
4. Copy the token
5. When pushing, use the token as password:
   ```powershell
   git push -u origin main
   # Username: your-github-username
   # Password: your-personal-access-token
   ```

### Option 2: GitHub CLI

```powershell
# Install GitHub CLI
# Then authenticate
gh auth login

# Push
git push -u origin main
```

### Option 3: SSH Keys

1. Generate SSH key:
   ```powershell
   ssh-keygen -t ed25519 -C "your_email@example.com"
   ```

2. Add to GitHub:
   - Copy public key: `cat ~/.ssh/id_ed25519.pub`
   - GitHub → Settings → SSH and GPG keys → New SSH key

3. Change remote URL:
   ```powershell
   git remote set-url origin git@github.com:YOUR_USERNAME/Renault-Smart-Center.git
   ```

## 📝 Project Structure on GitHub

Your repository will include:

```
Renault-Smart-Center/
├── .github/
│   └── workflows/
│       └── dotnet.yml          # CI/CD pipeline
├── src/
│   ├── RenaultSmartCenter.Domain/
│   ├── RenaultSmartCenter.Application/
│   ├── RenaultSmartCenter.Infrastructure/
│   ├── RenaultSmartCenter.API/
│   └── RenaultSmartCenter.BlazorUI/
├── .gitignore                  # Git ignore rules
├── .gitattributes              # Git attributes
├── LICENSE                     # MIT License
├── README.md                   # Project documentation
├── ARCHITECTURE.md             # Architecture details
├── DATABASE.md                 # Database documentation
└── RenaultSmartCenter.sln      # Solution file
```

## ⚠️ Important Notes

### Sensitive Data
**Never commit these files:**
- `appsettings.json` with real connection strings or secrets
- `appsettings.Production.json` with production secrets
- Any files containing API keys or passwords

**Instead:**
- Use `appsettings.Development.json` for local development (add to .gitignore)
- Use environment variables or Azure Key Vault for production
- Use user secrets for development: `dotnet user-secrets init`

### Connection Strings
Before pushing, make sure your `appsettings.json` doesn't contain production connection strings:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=RenaultSmartCenter;..."
  }
}
```

This is fine for local development, but create a template file for other developers.

## 🔄 Future Updates

To push changes in the future:

```powershell
# Check status
git status

# Add changes
git add .

# Commit with message
git commit -m "Description of changes"

# Push to GitHub
git push
```

## 📚 Additional Resources

- [Git Documentation](https://git-scm.com/doc)
- [GitHub Docs](https://docs.github.com/)
- [.NET User Secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets)

---

**Happy Coding! 🚀**

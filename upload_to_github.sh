#!/bin/bash

# Elite & Ultimate Variants - GitHub Upload Script
# This script creates a GitHub repository and uploads the mod

set -e  # Exit on error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}Elite & Ultimate Variants - GitHub Upload Script${NC}"
echo "=================================================="
echo ""

# Get mod directory
MOD_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
echo -e "Mod directory: ${YELLOW}$MOD_DIR${NC}"
echo ""

# Check if git is installed
if ! command -v git &> /dev/null; then
    echo -e "${RED}Error: git is not installed. Please install git first.${NC}"
    exit 1
fi

# Check if gh CLI is installed
GH_CLI=false
if command -v gh &> /dev/null; then
    GH_CLI=true
    echo -e "${GREEN}GitHub CLI (gh) detected - will use for easier upload${NC}"
else
    echo -e "${YELLOW}GitHub CLI (gh) not detected - will use git with manual token${NC}"
    echo "Tip: Install gh CLI for easier uploads: https://cli.github.com/"
fi
echo ""

# Get repository information
echo "Enter repository information:"
read -p "GitHub username: " GITHUB_USER
read -p "Repository name (e.g., elite-ultimate-variants): " REPO_NAME
read -p "Make repository public? (y/n): " IS_PUBLIC

if [[ "$IS_PUBLIC" == "y" || "$IS_PUBLIC" == "Y" ]]; then
    VISIBILITY="public"
else
    VISIBILITY="private"
fi

echo ""
echo -e "Repository will be created as: ${YELLOW}$GITHUB_USER/$REPO_NAME${NC} ($VISIBILITY)"
echo ""

# Initialize git repository if not already initialized
if [ ! -d "$MOD_DIR/.git" ]; then
    echo "Initializing git repository..."
    cd "$MOD_DIR"
    git init
    git branch -M main
    echo -e "${GREEN}Git repository initialized${NC}"
else
    echo -e "${YELLOW}Git repository already initialized${NC}"
    cd "$MOD_DIR"
fi

# Create .gitattributes for proper line endings
cat > .gitattributes << 'EOF'
# Auto detect text files and perform LF normalization
* text=auto

# C# files
*.cs text diff=csharp

# XML files
*.xml text

# Markdown files
*.md text

# Scripts
*.sh text eol=lf
*.bat text eol=crlf
EOF

echo ""

# Add files to git
echo "Adding files to git..."
git add .
git status
echo ""

# Commit
read -p "Enter commit message (default: 'Initial commit - Elite & Ultimate Variants v1.0.0'): " COMMIT_MSG
if [ -z "$COMMIT_MSG" ]; then
    COMMIT_MSG="Initial commit - Elite & Ultimate Variants v1.0.0"
fi

git commit -m "$COMMIT_MSG" || echo "Nothing to commit or already committed"
echo -e "${GREEN}Files committed${NC}"
echo ""

# Create GitHub repository and push
if [ "$GH_CLI" = true ]; then
    echo "Creating GitHub repository using gh CLI..."
    echo "You may be prompted to authenticate if not already logged in."
    echo ""

    # Check if logged in
    if ! gh auth status &> /dev/null; then
        echo "Please log in to GitHub:"
        gh auth login
    fi

    # Create repository
    if [ "$VISIBILITY" = "public" ]; then
        gh repo create "$GITHUB_USER/$REPO_NAME" --public --source=. --remote=origin --push
    else
        gh repo create "$GITHUB_USER/$REPO_NAME" --private --source=. --remote=origin --push
    fi

    echo -e "${GREEN}Repository created and code pushed!${NC}"
    echo ""
    echo -e "Repository URL: ${YELLOW}https://github.com/$GITHUB_USER/$REPO_NAME${NC}"

else
    echo "Creating GitHub repository manually..."
    echo ""
    echo "Please create the repository manually:"
    echo "1. Go to https://github.com/new"
    echo "2. Repository name: $REPO_NAME"
    echo "3. Visibility: $VISIBILITY"
    echo "4. Do NOT initialize with README, .gitignore, or license"
    echo "5. Click 'Create repository'"
    echo ""
    read -p "Press Enter after creating the repository..."

    # Get personal access token
    echo ""
    echo "You need a Personal Access Token with 'repo' permissions."
    echo "Create one at: https://github.com/settings/tokens/new"
    echo ""
    read -sp "Enter your GitHub Personal Access Token: " GITHUB_TOKEN
    echo ""

    # Add remote
    REMOTE_URL="https://$GITHUB_TOKEN@github.com/$GITHUB_USER/$REPO_NAME.git"

    if git remote | grep -q '^origin$'; then
        git remote set-url origin "$REMOTE_URL"
    else
        git remote add origin "$REMOTE_URL"
    fi

    # Push
    echo "Pushing to GitHub..."
    git push -u origin main

    echo -e "${GREEN}Code pushed to GitHub!${NC}"
    echo ""
    echo -e "Repository URL: ${YELLOW}https://github.com/$GITHUB_USER/$REPO_NAME${NC}"
fi

# Create release
echo ""
read -p "Create a v1.0.0 release? (y/n): " CREATE_RELEASE

if [[ "$CREATE_RELEASE" == "y" || "$CREATE_RELEASE" == "Y" ]]; then
    if [ "$GH_CLI" = true ]; then
        echo "Creating release v1.0.0..."
        gh release create v1.0.0 \
            --title "Elite & Ultimate Variants v1.0.0" \
            --notes "Initial release of Elite & Ultimate Variants mod for Caves of Qud.

## Features
- Elite and Ultimate variant system
- 5 difficulty presets with smart scaling
- Elite army spawning with 5 army types
- Automatic and manual preset application
- 40+ wish commands for customization and testing
- Comprehensive options menu with combo box interface
- Natural spawning with safety limits
- Debug and testing tools

## Installation
Download the source code and extract to your Caves of Qud mods directory.

See README.md for full documentation." \
            --latest

        echo -e "${GREEN}Release v1.0.0 created!${NC}"
    else
        echo ""
        echo "To create a release manually:"
        echo "1. Go to https://github.com/$GITHUB_USER/$REPO_NAME/releases/new"
        echo "2. Tag: v1.0.0"
        echo "3. Title: Elite & Ultimate Variants v1.0.0"
        echo "4. Copy description from STEAM_DESCRIPTION.txt"
        echo "5. Click 'Publish release'"
    fi
fi

echo ""
echo -e "${GREEN}GitHub upload complete!${NC}"
echo ""
echo "Next steps:"
echo "1. Visit your repository: https://github.com/$GITHUB_USER/$REPO_NAME"
echo "2. Add topics/tags (caves-of-qud, mod, roguelike)"
echo "3. Verify README displays correctly"
echo "4. (Optional) Set up GitHub Pages for documentation"
echo ""
echo "For Steam Workshop upload, see STEAM_UPLOAD_GUIDE.md"

#!/bin/bash
set -e

echo "🚀 Setting up .NET MAUI Development Environment..."

# Install MAUI workload
echo "📦 Installing .NET MAUI workload..."
dotnet workload install maui --skip-sign-check

# Install Android workload for Android builds
echo "📱 Installing Android workload..."
dotnet workload install android --skip-sign-check

# Restore project dependencies
echo "🔄 Restoring project dependencies..."
dotnet restore

# Build project to verify setup
echo "🔨 Building project..."
dotnet build --configuration Debug

echo "✅ Development environment setup complete!"
echo "💡 You can now build the project with:"
echo "   - Android: dotnet build -f net10.0-android"
echo "   - Windows: dotnet build -f net10.0-windows10.0.19041.0"
echo "   - Run tests: dotnet test"

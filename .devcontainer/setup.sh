#!/bin/bash
set -e

echo "🚀 Setting up .NET MAUI Development Environment..."

# Valid values are only '18.04', '20.04', '22.04', and '24.04'
# For other versions of Ubuntu, please use the tar.gz package
ubuntu_release=$(lsb_release -rs)
wget https://packages.microsoft.com/config/ubuntu/${ubuntu_release}/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb


# Install Android workload
echo "📦 Installing android workload..."
dotnet workload install android

dotnet build src/Android/Android.csproj -t:InstallAndroidDependencies -f net10.0-android -p:AndroidSdkDirectory=/usr/lib/android-sdk -p:JavaSdkDirectory=/usr/lib/jdk -p:AcceptAndroidSdkLicenses=True

# Restore project dependencies
echo "🔄 Restoring project dependencies..."
dotnet restore src/LuebeckRegatta.slnx

# Build project to verify setup
echo "🔨 Building project..."
#dotnet build --configuration Debug

echo "✅ Development environment setup complete!"
echo "💡 You can now build the project with:"
echo "   - Android: dotnet build -f net10.0-android"
echo "   - Windows: dotnet build -f net10.0-windows10.0.19041.0"
echo "   - Run tests: dotnet test"

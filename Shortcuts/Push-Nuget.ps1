$dir="$PWD"
pushd ..\Universe.GenericTreeTable\

$commits=(& git.exe log -n 999999 --date=raw --pretty=format:"%cd").Split(13).Length
$SELF_VERSION="1.1.$commits"
echo "SELF_VERSION: $SELF_VERSION"

dotnet pack -c Release -p:PackageVersion=$SELF_VERSION -p:Version=$SELF_VERSION -p:IncludeSymbols=True -p:SymbolPackageFormat=snupkg -o "$dir\bin"
popd
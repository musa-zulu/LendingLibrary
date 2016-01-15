require "./rake-tasks/init"

#-----------------------------------SETTINGS-----------------------------------

$binaries_baselocation = "bin"
$binaries_location = "#{$binaries_baselocation}/release"
$nunittesting_location = "#{$binaries_baselocation}/nunittesting"

#--------------------------------build settings--------------------------------

$build_configuration = "debug"
$app_version = "0.3"            #Major version
$release_type = "Debug"         #Debug, Staging or Production

def msbuild_settings
    {
      :properties => {:configuration => $build_configuration},
      :targets => [:clean, :rebuild],
      :verbosity => :minimal,
      :parameters => [ "/m", "/p:TargetProfile=Local", "/toolsVersion:14.0", "/p:VisualStudioVersion=14.0" ],
    }
end

#------------------------dependency settings---------------------

#------------------------project settings------------------------
$solution = "source/LendingLibrary.sln"
#---- c# testing ------------------------------------------------
$binaries_baselocation = "source/bin"
$nuget_location = "../rake-tasks"
$binaries_location = "#{$binaries_baselocation}/all"
$nunittesting_location = "#{$binaries_baselocation}/nunittesting"
#---- javascript testing ----------------------------------------
$webproject_baselocation = "source"
$webproject_application = "LendingLibrary.Web"
$webproject_apptestlocation = "LendingLibrary.Web.Tests"
#______________________________________________________________________________
#---------------------------------TASKS----------------------------------------
desc "Runs the build all task"
task :default => [:build]

desc "Builds including tests"
task :build => [:just_build, :test]

desc "Builds without tests"
task :just_build => [:update_packages, :clean, :msbuild, :copy_to_bin]

desc "Builds with coverage and js checks"
task :build_with_cover => [:just_build, :test_with_nunit_coverage, :test_with_jasmine_coverage, :check_js] 

desc "Builds with coverage, js checks and sonar"
task :build_with_sonar => [:just_build, :test_with_nunit_coverage, :test_with_jasmine_coverage, :check_js, :sonar]
#---------------------------------Update stuff---------------------------------

desc "Updates packages with nuget"
updatenugetpackages :update_packages do |nuget|
    puts cyan("Updating nuget packages")
    nuget.solution = $solution
end

if not File.directory?('buildreports')
    Dir.mkdir('buildreports')
end

#------------------------build   --------------------

desc "Cleans the bin folder"
task :clean do
	puts cyan("Cleaning bin folder")
	FileUtils.rm_rf 'bin'
end

def setupMsBuildOn msbuildTask
    workingMsBuild = "C:/Program Files (x86)/MSBuild/12.0/Bin/msbuild.exe"
    if File.exists?(workingMsBuild)
        msbuildTask.command = workingMsBuild
    end
    puts yellow ("Using msbuild at: " + msbuildTask.command)

end

desc "Builds the solution with msbuild"
msbuild :msbuild do |msb|
                puts cyan("Building #{$solution} with msbuild")
                msb.update_attributes msbuild_settings
                msb.solution = $solution
    setupMsBuildOn msb
end

task :copy_to_bin => [:discover_test_projects] do
	puts cyan("Copying files to the bin folder")
	FileSystem.EnsurePath $binaries_baselocation
	
	FileSystem.EnsurePath $nunittesting_location
	copy_nunittesting_files_to $nunittesting_location
	
end


#---------------------------------NUnit tests----------------------------------
def copy_test_files source, target
	puts cyan("Copying test files ")
	FileSystem.EnsurePath target
	FileSystem.CopyExecutables source, target
	FileSystem.CopyDlls source, target
	targetInputFiles = File.join(target,"TestFiles")
	FileSystem.EnsurePath targetInputFiles
	FileSystem.CopyWithFilter source, target, "*.txt"
	FileSystem.CopyWithFilter source, targetInputFiles, "*.txt"
	FileSystem.CopyWithFilter source, targetInputFiles, "*.xml"
	FileSystem.CopyWithFilter source, target, "*.pdb"
	sourceInputFiles = File.join(source,"TestFiles")
	FileSystem.CopyWithFilter sourceInputFiles, targetInputFiles, "*.*"
end

def copy_nunittesting_files_to location	
	puts cyan("Testing build config: #{$build_configuration}")
	for project in $test_projects
		copy_test_files project, location
	end  
end

task :discover_test_projects do
	$all_dirs = Dir.glob('**/*').select {|f| File.directory? f}
	$test_projects = $all_dirs.select {|t| (t.match(/[.]Tests\z/) || t.match(/[.]Specs\z/)) && Dir.exists?(File.join(t, "bin", $build_configuration ))}	
	$test_dlls = $test_projects.collect {|s| s.split("/").last}
end

#----------------------------------Run Tests---------------------------------
def testassemblies
    $test_dlls.map {|a| File.join($nunittesting_location, a + ".dll")}
end

desc "Runs the tests"
nunit :test => [:discover_test_projects] do |nunit|
    puts cyan("Running tests")
    nunit.assemblies testassemblies
end

desc "Runs the tests with dotcover"
dotcover :test_with_nunit_coverage do |dc|
	puts cyan("Running tests with dotcover")
	dc.assemblies testassemblies
    dc.filters '+:module=*;class=*;function=*;-:*.Tests;-:*.Tests.Common'
end
#---------------------------------Run Web Tests--------------------------------

npm :update_node_packages do |npm|
    npm.base = $webproject_baselocation
end

desc "Launch Karma continuous web test runner"
karma :karma do |karma|
    puts cyan("Launching Karma continuous web test runner (Ctrl+C to stop)")
    karma.base = $webproject_baselocation
    karma.browsers = "Chrome"
end

desc "Run web tests once"
karma :test_web do |karma|
    puts cyan("Running web tests with Karma")
    karma.base = $webproject_baselocation
    karma.singlerun = true
    karma.browsers = "PhantomJS,Chrome"
end

desc "Run web tests once with coverage"
karma :test_with_jasmine_coverage => [:test_web] do |karma|
    puts cyan("Running web coverage with Karma")
    karma.base = $webproject_baselocation
    karma.coverage = true
    karma.browsers = "Chrome"
end

#----------------------------------Build Stats---------------------------------

jslint :jslint_no_output do |lint|
    puts cyan("Running JSLint")
    lint.base = $webproject_baselocation
    lint.source = ["#{$webproject_application}/js"]
end

desc "JSLint with results listed"
jslintoutput :jslint => [:jslint_no_output] do |lint|
    lint.base = $webproject_baselocation
end

desc "Checkstyle"
jslint :checkstyle do |lint|
    puts cyan("Running Checkstyle")
    lint.checkstyle = true
    lint.base = $webproject_baselocation
    lint.source = ["#{$webproject_application}/js"]
end

desc "Plato"
plato :plato do |plato|
    puts cyan("Running Plato")
    plato.base = $webproject_baselocation
    plato.source = ["#{$webproject_application}/js"]
end

desc "JSLint, Checkstyle and Plato"
task :check_js => [:jslint_no_output, :checkstyle, :plato] do
end

desc "Runs sonar"
exec :sonar do |cmd|
  puts cyan("Running Sonar")
  cmd.command = "cmd.exe"
  cmd.parameters = "/c #{$sonar_runner_path}"
end

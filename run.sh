# backup code etc. goes above here

runDir=`pwd`

cd server-runtime
while true
do
    # java -jar goes here
    clear
    echo "Running the crash notification service."
    dotnet "PATH_TO_CRASHALERT_DLL" $runDir "WEBHOOK_ID" "WEBHOOK_TOKEN" #do "dotnet Mmcc.CrashAlert.dll --help" for more info
	echo "Restarting server in 5 seconds. Press [CTRL+C] to cancel."
	sleep 5
done
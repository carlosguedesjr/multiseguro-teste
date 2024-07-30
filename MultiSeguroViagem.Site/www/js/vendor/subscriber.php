<?php
$useragent = $_SERVER['HTTP_USER_AGENT'];
 
  if (preg_match('|MSIE ([0-9].[0-9]{1,2})|',$useragent,$matched)) {
    $browser_version=$matched[1];
    $browser = 'IE';
  } elseif (preg_match( '|Opera/([0-9].[0-9]{1,2})|',$useragent,$matched)) {
    $browser_version=$matched[1];
    $browser = 'Opera';
  } elseif(preg_match('|Firefox/([0-9\.]+)|',$useragent,$matched)) {
    $browser_version=$matched[1];
    $browser = 'Firefox';
  } elseif(preg_match('|Chrome/([0-9\.]+)|',$useragent,$matched)) {
    $browser_version=$matched[1];
    $browser = 'Chrome';
  } elseif(preg_match('|Safari/([0-9\.]+)|',$useragent,$matched)) {
    $browser_version=$matched[1];
    $browser = 'Safari';
  } else {
    // browser not recognized!
    $browser_version = 0;
    $browser= 'other';
  }
  $navegador = $browser . $browser_version;


$iphone = strpos($_SERVER['HTTP_USER_AGENT'],"iPhone");
$ipad = strpos($_SERVER['HTTP_USER_AGENT'],"iPad");
$android = strpos($_SERVER['HTTP_USER_AGENT'],"Android");
$palmpre = strpos($_SERVER['HTTP_USER_AGENT'],"webOS");
$berry = strpos($_SERVER['HTTP_USER_AGENT'],"BlackBerry");
$ipod = strpos($_SERVER['HTTP_USER_AGENT'],"iPod");
$symbian =  strpos($_SERVER['HTTP_USER_AGENT'],"Symbian");
$aparelho = "Computador";
if ($iphone == true) 
{
    $aparelho = "iPhone";
} 
if ($iPad == true) 
{
    $aparelho = "iPad";
} 
if ($android == true) 
{
    $aparelho = "Android";
} 
if ($palmpre == true) 
{
    $aparelho = "Palmpre";
} 
if ($berry == true) 
{
    $aparelho = "Berry";
} 
if ($ipod == true) 
{
    $aparelho = "Ipod";
} 
if ($symbian == true) 
{
    $aparelho = "Symbian";
} 


$spath = str_replace("\\","/",getcwd()).'/';
$servroot = rtrim($_SERVER['DOCUMENT_ROOT'],'/').'/';
if ( !empty( $_SERVER['HTTPS'] ) ) {
	$hostroot = 'https://'.$_SERVER['HTTP_HOST'].'/';
}else{
	$hostroot = 'http://'.$_SERVER['HTTP_HOST'].'/';
}
$hpath = str_replace($servroot, $hostroot, $spath);
$name = basename(__FILE__, '.php'); 
$fname = basename(__FILE__);

$isdb = false;
$servername = "10.100.17.44";
$username = "root";
$password = "PtyoFwEgxy";

try{
	   $db = new PDO("mysql:host=$servername;dbname=WebPush", $username, $password);
    $db->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
}
catch(PDOException $e){
	 exit($e->getMessage());
}

$publickey = 'BOEXAaJGnfsYu4YNVNdozYT9_TbsGKRxnB_qRzC0vPjBkG31GbylZs3krSYHxqYTsrRi8yahH503sYSxaWArjBg';
$privatekey = 'wc6tHXAVPCBcjEiz1qEiaH9OO8MzcXgkcBF-5VGqotw';

$_POST = json_decode(file_get_contents('php://input'), true); //for php 7


if(isset($_POST['axn']) && $_POST['axn'] != NULL){
	$output = '';
	switch($_POST['axn']){
		case "subscribe":
			//filter out bad data
			$myQuery = "SELECT * FROM subscribers WHERE endpoint = ".$db->quote($_POST['endpoint']);
			try{
			    $result = $db->query($myQuery)->fetch(PDO::FETCH_ASSOC);
			    if($result['id'] == NULL || $result['id'] == ""){
					$my_query = "REPLACE INTO subscribers (endpoint, p256dh, auth, idservico, ip, navegador, dominio, aparelho) VALUES (".$db->quote($_POST['endpoint']).", ".$db->quote($_POST['key']).", ".$db->quote($_POST['token']).", ". 1 .",'" .$_SERVER['REMOTE_ADDR'] ."','".$navegador."','".$_SERVER['HTTP_HOST']."','".$aparelho."'); ";
					    echo $my_query.'<BR><BR>';
					    try {
					        $db->exec($my_query);
					        }
					    catch(PDOException $e)
					        {
					        echo $e->getMessage();
					        }
					    //$i++;
					    $output .= 'adding user <br>';
		   	    }else{
			        $output .= 'user exists in db :  <br>';
			        
			    }
			}
			catch(PDOException $e){
			     exit($e->getMessage());
			} 
			echo $output;
			exit;
			break;
		case "unsubscribe":
			$myQuery = "SELECT * FROM subscribers WHERE endpoint = ".$db->quote($_POST['endpoint']);
			try{
			    $result = $db->query($myQuery)->fetch(PDO::FETCH_ASSOC);
			    if($result['id'] != NULL){
					$my_query = "DELETE FROM subscribers WHERE endpoint = ".$db->quote($_POST['endpoint']);
					   // exit($my_query);
					    try {
					        $db->exec($my_query);
					        }
					    catch(PDOException $e)
					        {
					        echo $e->getMessage();
					        }
					    $output .= 'removing user <br>';
		   	    }else{
			        $output .= 'user does not exist in db :  <br>';
			        
			    }
			}
			catch(PDOException $e){
			     exit($e->getMessage());
			} 
			echo $output;
			exit;
			break;
		default:
	}
	exit;
}
?>
<script>
		var aspkey = '<?php echo $publickey; ?>';
</script>
<button class="pushtoglbtn" hidden >Enable Push Messaging</button><br><br>
<button class="sendpushbtn" hidden  >Send push notification</button><br><br>
<script src="main.js"></script>


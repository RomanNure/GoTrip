<?php
	header("Access-Control-Allow-Origin: *");

	$file = $_FILES['company_avatar'];	
	file_put_contents("company_log.txt", "error: ".$file['error']."\n", FILE_APPEND);

	$company_dir = 'imgs/companies/';	
	$company_avatar_file_path = $company_dir.uniqid().".png";
	
	if(move_uploaded_file($file['tmp_name'], $company_avatar_file_path))
		file_put_contents("company_log.txt", "company avatar file is in ".$company_avatar_file_path."\n", FILE_APPEND);
	else
		file_put_contents("company_log.txt", "company avatar uploading failed\n", FILE_APPEND);
	
	$response = '{ "path" : "http://vvkyrychenko.zzz.com.ua/gotrip/'.$company_avatar_file_path.'"}';
	file_put_contents("company_log.txt", $response." was responsed\n", FILE_APPEND);
	
	echo $response;
?>
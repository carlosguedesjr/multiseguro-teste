<?php
$xml = '<sitemapindex xmlns="http://www.sitemaps.org/schemas/sitemap/0.9">';
$DateTime = new DateTime('Today');
$template = '<sitemap><loc>https://www.multiseguroviagem.com.br/sitemaps/{{file}}</loc><lastmod>' . $DateTime->format('Y-m-d') . '</lastmod></sitemap>';

$files = glob('./*.{xml}', GLOB_BRACE);
foreach($files as $file) {
	$xml .= str_replace('{{file}}', str_replace('./', '', $file), $template);
}

$xml .= '</sitemapindex>';

$seoIndex = fopen("./seo-index.xml", "w");
fwrite($seoIndex, $xml);
fclose($seoIndex);
?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:ms="urn:schemas-microsoft-com:xslt"
  xmlns:ditaarch="http://dita.oasis-open.org/architecture/2005/"
  xmlns:tns="urn:com:ffx:eregs:filenet:quark:migration"
  xmlns:helpers="urn:com:ffx:eregs:filenet:quark:migration:helpers"
  exclude-result-prefixes="ditaarch ms tns helpers">

  <ms:script language="JScript" implements-prefix="helpers">
    function replace(str,find, replace) {
    return str.replace(new RegExp(find, 'g'), replace);
    }

    function encodeAsURI(value){
    return encodeURI(value);
    }
    
    
  </ms:script>
  
    <xsl:output method="xml" indent="yes"/>

    <xsl:template match="@* | node()">
        <xsl:copy>
            <xsl:apply-templates select="@* | node()"/>
        </xsl:copy>
    </xsl:template>
  <xsl:template match="p">
    <xsl:choose>
      <xsl:when test="text()"
    </xsl:choose>
  </xsl:template>
</xsl:stylesheet>

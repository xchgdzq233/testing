<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <xsl:for-each select="/Page">
      <pre>
        <xsl:value-of select="text()"/>
      </pre>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
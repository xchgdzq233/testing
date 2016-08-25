<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <xsl:for-each select="Page">
      <div>
        <xsl:for-each select="Line">
          <p>
            <xsl:attribute name="style">
              <xsl:value-of select="concat('font-family:', @FontFamily)"/>
            </xsl:attribute>
            <xsl:value-of select="text()"/>
          </p>
        </xsl:for-each>
      </div>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
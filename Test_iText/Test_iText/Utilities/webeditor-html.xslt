<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <xsl:for-each select="Page">
      <section>
        <xsl:attribute name="style">
          <xsl:value-of select="@style"/>
        </xsl:attribute>
        <xsl:for-each select="Line">
          <p>
            <xsl:attribute name="style">
              <xsl:value-of select="@style"/>
            </xsl:attribute>
            <xsl:value-of select="text()"/>
          </p>
        </xsl:for-each>
      </section>
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
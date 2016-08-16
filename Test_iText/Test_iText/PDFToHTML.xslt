<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:template match="/">
    <html>
      <body>
        <h2>
          <xsl:value-of select="Document/@DocName"/>
        </h2>
        <xsl:for-each select="Document/Page">
          <pre>
            <xsl:value-of select="text()"/>
          </pre>
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>


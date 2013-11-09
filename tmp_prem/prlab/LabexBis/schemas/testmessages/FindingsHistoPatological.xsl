<?xml version="1.0" ?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/Findings">
    <html>
      <head>

        <style type="text/css">

          .bold {
            font-weight: bold;
          }

          table {
            width: 90%;
            table-layout: fixed;
          }

          body {
            margin-left: 1cm;
          }

        </style>

        <title>
          <xsl:value-of select="/Head/Heading"/>
        </title>
      </head>


      <body>

        <xsl:apply-templates select="Head" />

        <hr />

        <xsl:apply-templates select="DiagnosisClinical" />

        <hr />

        <xsl:apply-templates select="FormerFindings" />

        <H3>
          <xsl:text>MAKROSKOPSKI OPIS:</xsl:text>
        </H3>
        <xsl:value-of select="MacroDescription"/>
        <br />

        <H3>
          <xsl:text>MIKROSKOPSKI OPIS:</xsl:text>
        </H3>
        <xsl:value-of select="MicroDescription"/>
        <br />

        <hr />

        <xsl:apply-templates select="DiagnosisPatoHistological" />

        <hr />
        <br />

        <xsl:apply-templates select="Head/Physician" />

      </body>

    </html>
  </xsl:template>

  <!-- glava izpisa -->
  <xsl:template match="Head">
    <H2>
      <xsl:value-of select="Heading"/>
    </H2>

    <table>
      <colgroup>
        <col style="width: 40px; font-weight: bold;" />
        <col style="width: 10px" />
        <col style="width: 400px" />
      </colgroup>

      <xsl:apply-templates select="Patient" />

      <tr>
        <td>
          <br />
        </td>
      </tr>

      <!-- podatki o narocniku -->
      <tr>
        <td>
          <xsl:text>Naročnik</xsl:text>
        </td>
        <td>
          <xsl:text>:</xsl:text>
        </td>
        <td>
          <xsl:value-of select="Orderer"/>
        </td>
      </tr>

      <tr>
        <td>
          <xsl:text>Naslov</xsl:text>
        </td>
        <td>
          <xsl:text>:</xsl:text>
        </td>
        <td>
          <xsl:value-of select="OrdererAddress"/>
        </td>
      </tr>
    </table>

    <br />

    <!-- podatki o casu... -->
    <table>
      <colgroup>
        <col style="width: 60px; font-weight: bold;" />
        <col style="width: 10px" />
        <col style="width: 300px" />
      </colgroup>

      <tr>
        <td>
          <xsl:text>Čas odvzema</xsl:text>
        </td>
        <td>
          <xsl:text>:</xsl:text>
        </td>
        <td>
          <xsl:value-of select="SampleTime"/>
        </td>
      </tr>

      <tr>
        <td>
          <xsl:text>Datum/ura sprejema</xsl:text>
        </td>
        <td>
          <xsl:text>:</xsl:text>
        </td>
        <td>
          <xsl:value-of select="ReceptionTime"/>
        </td>
      </tr>

      <tr>
        <td>
          <xsl:text>datum zaključitve</xsl:text>
        </td>
        <td>
          <xsl:text>:</xsl:text>
        </td>
        <td>
          <xsl:value-of select="CompletionTime"/>
        </td>
      </tr>

    </table>

    <br />

  </xsl:template>

  <!-- podatki o pacientu -->
  <xsl:template match="Patient">
    <tr>
      <td>
        <xsl:text>Priimek/ime</xsl:text>
      </td>
      <td>
        <xsl:text>:</xsl:text>
      </td>
      <td>
        <b>
          <xsl:value-of select="FamilyName"/>
          <xsl:text> </xsl:text>
          <xsl:value-of select="Name"/>
          <xsl:text>, rojen: </xsl:text>
          <xsl:value-of select="DateOfBirth"/>
        </b>
        <xsl:text> (</xsl:text>
        <xsl:value-of select="Sex"/>
        <xsl:text>, </xsl:text>
        <xsl:value-of select="Age"/>
        <xsl:text>, </xsl:text>
        <xsl:value-of select="AgeMonths"/>
        <xsl:text>)</xsl:text>
      </td>
    </tr>

    <tr>
      <td>
        <xsl:text>Naslov</xsl:text>
      </td>
      <td>
        <xsl:text>:</xsl:text>
      </td>
      <td>
        <xsl:value-of select="Address"/>
      </td>
    </tr>
  </xsl:template>

  <!-- klinicni podatki/diagnoze -->
  <xsl:template match="DiagnosisClinical">
    <H3>
      <xsl:text>KLINIČNI PODATKI/DIAGNOZE:</xsl:text>
    </H3>

    <xsl:value-of select="Text"/>
    <br />
    <br />

    <b>
      <xsl:text>Šifre diagnoz: </xsl:text>
    </b>

    <xsl:variable name="numCodes">
      <xsl:value-of select="count(Codes/DiagnosisCode)"/>
    </xsl:variable>

    <xsl:for-each select="Codes/DiagnosisCode">
      <xsl:value-of select="Code"/>
	  
	  <!-- da ne bo vejice na koncu... -->
      <xsl:if test="position() &lt; $numCodes">
        <xsl:text>, </xsl:text>
      </xsl:if>

    </xsl:for-each>

    <br />

  </xsl:template>

  <!-- prejsnji izvidi -->
  <xsl:template match="FormerFindings">
    <br />
    <b>
      <xsl:text>Prejšnji izvidi: </xsl:text>
    </b>

    <xsl:variable name="numFindings">
      <xsl:value-of select="count(Finding)"/>
    </xsl:variable>
    
    <xsl:for-each select="Finding">
      <xsl:value-of select="FindingReference"/>
      
      <xsl:if test="position() &lt; $numFindings">
        <xsl:text>, </xsl:text>
      </xsl:if>

    </xsl:for-each>
  </xsl:template>

  <!-- zacasne diagnoze, mnenja -->
  <xsl:template match="DiagnosisPatoHistological">
    <H3>
      <xsl:text>ZAČASNE DIAGNOZE/MNENJE:</xsl:text>
    </H3>

    <xsl:value-of select="Text"/>
    <br />
    <br />

    <b>
      <xsl:text>Šifre diagnoz: </xsl:text>
    </b>

    <xsl:variable name="numCodes">
      <xsl:value-of select="count(Codes/DiagnosisCode)"/>
    </xsl:variable>
    
    <xsl:for-each select="Codes/DiagnosisCode">
      <xsl:value-of select="Code"/>

      <xsl:if test="position() &lt; $numCodes">
        <xsl:text>, </xsl:text>
      </xsl:if>
      
    </xsl:for-each>
  </xsl:template>

  <!-- podatki o zdravniku -->
  <xsl:template match="Head/Physician">
    <div style="margin-left: 2cm;">
      <b>
        <xsl:value-of select="Appellation"/>
        <xsl:text>: </xsl:text>
      </b>
      <xsl:value-of select="Title"/>
      <xsl:text> </xsl:text>
      <xsl:value-of select="Name"/>
      <xsl:text> </xsl:text>
      <xsl:value-of select="FamilyName"/>
      <xsl:text>, </xsl:text>
      <xsl:value-of select="TitleRight"/>
    </div>
  </xsl:template>

</xsl:stylesheet>


              /**************************************/
              /*                                    */
              /*   SAMPLE DIRECT ANALYSIS PROGRAM   */
              /*                                    */
              /**************************************/


#include <stdlib.h>
#include <stdio.h>
#include <string.h>
#include <stddef.h>
#include <math.h>


/***************************************************************************
  This is an example of the direct analysis program that uses the uniform
file interface to exchange data with an optimization algorithm. It can be
used as a template to program other analysis programs.
  The program is run as
    program_name infilename outfilename <which <printlevel>>
where infilename is the name of the input file from which optimization
parameters and calculation request flags are read, outfilename is the name
of the output file to which the program writes analysis results (response
functions - objective and constraint - and its gradients, calculation flags
and return code), which is an optional integer parameter used to choose
between a couple of built in analyses, and printlevel is an optional integer
parameter that defines the level of information that is printed on the
terminal.
  See the documentation for Inverse, document on optimization utilities,
section on uniform file interface, subsection on example.
***************************************************************************/

int printlevel=3;





int readanalysisinput(char *infilename,double *param,int maxbuf,int *ptrnumparam,
    int *reqcalcobj,int *reqcalcconstr,int *reqcalcgradobj,int *reqcalcgradconstr)
    /* Reads input data for analysis (parameter and calculation request flags).
      infilename - name of the input file
      param - storage for parameters
      maxbuf - allocated size of param (maximal number of parameters that can
      be read)
      ptrnumparam - Pointer to the location where the number od parameters is
    stored. *ptrnumparam must contain number of parameters or 0 if the number
    is to be defined after reading. Actual number of parameters read from the
    file is stored in *param at function return.
      reqcalcobj, reqcalcconstr, reqcalcgradobj, reqcalcgradconstr - pointers
    to locations where calculation flags are to be stored.
    $A Igor oct06; */
{
static int sizebuf=0;
int numparam,   
    numread,numbrac,numc=0,num=0,i=0,
    ret=0,
    ibuf[10];
double daux;
FILE *infile=NULL;
char ch;

if (param==NULL)
{
  ret=-1;
  printf("\n\nERROR: Storage for parameters is NULL.\n\n");
} else if (ptrnumparam==NULL)
{
  ret=-2;
  printf("\n\nERROR: Pointer to number of parametes is NULL.\n\n");
} else if (reqcalcobj==NULL || reqcalcconstr==NULL || reqcalcgradobj==NULL || 
         reqcalcgradconstr==NULL)
{
  ret=-3;
  printf("\n\nERROR: One or more of calculation flag pointers are NULL.\n\n");
} else
{
  numparam=*ptrnumparam;  /* Number of parameters */
  infile=fopen(infilename,"rb");

  if (infile==NULL)
  {
    ret=-4;
    printf("\n\nERROR: Could not open the input file \"%s\" for reading.\n\n",
        infilename);
  }  else
  {
    /* READ DATA FROM INPUT FILE (parameters & flags): */
    /* Jump into the second bracket: */
    numbrac=0;
    while(numbrac<2 && !feof(infile) )
    {
      ch=fgetc(infile);
      if (ch=='{')
        ++numbrac;
    }
    /* Read parameters: */
    num=0;
    numread=1;
    while(numread==1 && !feof(infile))
    {
      if (num<maxbuf)
        numread=fscanf(infile,"%lg",param+num);
      else
        numread=fscanf(infile,"%lg",&daux);
      if (numread==1)
      {
        if (num<maxbuf)
        {
          if (printlevel>1)
            printf("Parameter %i read, value = %g \n",num+1,param[num]);
          ++num;
        }
        ch=' ';
        while(ch!='}' && ch!=',')
          ch=fgetc(infile);
        if (ch=='}')
          numread=0;
        if (num>=maxbuf)
        {
          ret=-5;
          numread=0;
          printf("\n\nERROR: More parameters than the buffer size (%i).\n\n",
              maxbuf);
        }
      } else
      {
        ret=-6;
        printf("\n\nERROR: Parameter No. %i could not be read.\n\n",
            num+1);
      }
    }
    /* Verify the number of parameters: */
    if (numparam>0)
    {
      if (num<numparam)
      {
         ret=-7;
         printf("\n\nERROR: Too few parameters (%i instead of %i).\n\n",
                num,numparam);
      } else if (num>numparam)
      {
        ret=-8;
        printf("\n\nERROR: Too many parameters (%i instead of %i), redundant parameters will be rejected.\n\n",
                num,numparam);
      }
    }
    numparam=num;
    /* Copy parameters: */
    for (i=0;i<numparam;++i)
      param[i]=param[i];
    /* Jump into the next bracket containing calculation flags: */
    numbrac=0;
    while(numbrac<1 && !feof(infile) )
    {
      ch=fgetc(infile);
      if (ch=='{')
        ++numbrac;
    }
    /* Read calculation flags: */
    num=0;
    numread=1;
    while(numread==1 && !feof(infile) && num<4)
    {
      numread=fscanf(infile,"%i",&ibuf[num]);
      if (numread==1)
      {
        if (printlevel>1)
          printf("Flag %i read, value = %i \n",num+1,ibuf[num]);
        ++num;
        ch=' ';
        while(ch!='}' && ch!=',')
          ch=fgetc(infile);
        if (ch=='}')
          numread=0;
      } else
      {
        ret=-9;
        printf("\n\nERROR: Flag No. %i could not be read.\n\n",num+1);
      }
    }
    if (num<4)
    {
      ret=-10;
      printf("\n\nERROR: Only %i of 4 calculation flags could be read.\n\n",num);
    }
    /* Copy the calculation request flags: */
    if (num>=1)  *reqcalcobj=ibuf[0];
    if (num>=2)  *reqcalcconstr=ibuf[1];
    if (num>=3)  *reqcalcgradobj=ibuf[2];
    if (num>=4)  *reqcalcgradconstr=ibuf[3];
    if (printlevel>=1)
    {
      printf("\nParameters: {");
      for (i=1;i<numparam;++i)
        printf("%g, ",param[i-1]);
      printf("%g} \n",param[numparam-1]);
      printf("Calculation flags: {%i,%i,%i,%i}\n\n",*reqcalcobj,*reqcalcconstr,
          *reqcalcgradobj,*reqcalcgradconstr);
    }
  }
  *ptrnumparam=numparam;
}

if (infile!=NULL)
  fclose(infile);
return ret;
}




int writeanalysisoutput(char *outfilename, int numparam, int numconstr,
     double *param, double obj, double *constr, double *gradobj,
     double *gradconstr, int calcobj, int calcconstr, int calcgradobj,
     int calcgradconstr, int retcode )
    /* Writes analysis results to the file named outfilename, in the standard
    IOptLib format.
      Arguments:
      outfilename - name of the file to which results are written.
      numparam - number of parameters
      numconstr - number of constraints
      param - optimization parameters (counting starts from 0)
      obj - value of the objective functions
      constr - values of constraint functions
      gradobj - gradient of the objective function
      gradconstr - gradients of the constraint functions; order: components of
    gradient of the first constr. func., comp. of gradient of the second
    constraint function, etc.
      calcobj, calcconstr, calcgradobj, calcgradconstr - evaluation flags (0 if
    the corresponding quantity has not been calculated, non-zero if it has been
    calculated.
    $A Igor oct06; */
{
int ret=0,i,j;
FILE *outfile=NULL;
outfile=fopen(outfilename,"wb");
if (outfile==NULL)
{
  ret=-11;
  printf("\n\nERROR: Could not open the output file \"%s\" for writing.\n\n",
      outfilename);
} else
{
  /* OUTPUT DIRECT ANALYSIS RESULTS: */
  fprintf(outfile,"{ ");  /* opening bracket */
  /* Parameters: */
  fprintf(outfile,"{");
  for (i=0;i<numparam;++i)
  {
    fprintf(outfile,"%.20g",param[i]);
    if (i<numparam-1)
      fprintf(outfile,", ");
  }
  fprintf(outfile,"}, ");
  /* Response functions with calculation flags: */
  fprintf(outfile,"{ ");
  /* Objective function: */
  fprintf(outfile,"%i, %g, ",calcobj,obj);
  /* Constraint functions: */
  fprintf(outfile,"%i, {",calcconstr);
  for (i=0;i<numconstr;++i)
  {
    fprintf(outfile,"%.20g",constr[i]);
    if (i<numconstr-1)
      fprintf(outfile,", ");
  }
  fprintf(outfile,"}, ");
  
  /* Gradient of the objective function: */
  fprintf(outfile,"%i, {",calcgradobj);
  for (i=0;i<numparam;++i)
  {
    fprintf(outfile,"%.20g",gradobj[i]);
    if (i<numparam-1)
      fprintf(outfile,", ");
  }
  fprintf(outfile,"}, ");
  
  /* Gradients of the constraint functions: */
  fprintf(outfile,"%i, {",calcgradconstr);
  for (i=0;i<numconstr;++i)
  {
    /* Grad. of constraint No. i: */
    fprintf(outfile,"{");
    for (j=0;j<numparam;++j)
    {
      fprintf(outfile,"%.20g",gradconstr[i*numparam+j]);
      if (j<numparam-1)
        fprintf(outfile,", ");
    }
    fprintf(outfile,"}");  /* close grad. constr. No. i */    
    if (i<numconstr-1)
      fprintf(outfile,", ");
  }
  fprintf(outfile,"} ");  /* close gradients of constraints */
  /* Return code: */
  fprintf(outfile,", %i ",ret);
  fprintf(outfile,"}");    /* close response data */
  /* Requirement flags: */
  fprintf(outfile,", {%i,%i,%i,%i}",calcobj,calcconstr,
      calcgradobj,calcgradconstr);
  fprintf(outfile," } ");  /* closing bracket */
}
if (outfile!=NULL)
  fclose(outfile);
return ret;
}




int analysis(char *infilename,char *outfilename)
    /* Analysis function that reads the input data (optimization parameters
    and calculation flags) from the file infile in standard IOptLib format, 
    calculates the response functions and their gradients, and writes the
    results to the output file file named outfilename.
      This analysis function represents the following two parameric nonlinear
    constraint optimization problem:
    min f(x,y)=x^2+y^4, subject to y>=(x-3)^6 and y>=17-x^2, 
    therefore f(x,y)=x^2+y^4, c_1(x,y)=(x-3)^6-y, c_2(x,y)=17-x^2-y.
    Two local solutions are x=1.6, y=1, f(x,y)=0.2
    and x=10.095,  y=2.9522,  f(x,y)=0.
    $A Igor sep06; */
{
#define MAXBUF 20   /* Buffer length */
#define NUMPAR 2   /* Number of parameters */
#define NUMCONSTR 2   /* Number of constraints */
int numparam=NUMPAR,
    numconstr=NUMCONSTR,
    numc=0,num=0,i=0,j=0,
    ret=0,retaux,
    calcobj=0,calcconstr=0,calcgradobj=0,calcgradconstr=0,
    reqcalcobj=0,reqcalcconstr=0,reqcalcgradobj=0,reqcalcgradconstr=0;
double x,y,obj=0,
       param[MAXBUF]={0},
       constr[NUMCONSTR]={0},
       gradobj[NUMPAR]={0},
       gradconstr[NUMCONSTR*NUMPAR]={0};
FILE *infile=NULL,*outfile=NULL;
double A=0.1, B=0.005, CX=0.2, CY=-1, TX=0.6, TY=1,
       fr,ff,fder;

obj=0; constr[0]=constr[1]=0;
gradobj[0]=gradobj[1]=0.;
gradconstr[0]=gradconstr[1]=gradconstr[2]=gradconstr[3]=0.;

if (printlevel>=1)
{
  printf("\nDefault direct analysis (2 param., 2 constr.),\n  input file: \"%s\", output file: \"%s\"\n",
        infilename,outfilename);
}

/* Read input data (optimization parameters & calculation request flags): */
ret=readanalysisinput(infilename,param,MAXBUF,&numparam,
    &reqcalcobj,&reqcalcconstr,&reqcalcgradobj,&reqcalcgradconstr);
calcobj=reqcalcobj;  calcconstr=reqcalcconstr;
calcgradobj=reqcalcgradobj;  calcgradconstr=reqcalcgradconstr;

/* CALCULATE RESPONSE FUNCTIONS: */
x=param[0];  y=param[1];
/* Auxiliary functions: */
fr=sqrt(A*(x-CX)*(x-CX)+B*(y-CY)*(y-CY));
ff=sin(fr); ff=ff*ff;  /* ff=sin(fr)^2 */
if (fr<1e-8)
{
  /* Replace sin(fr)/fr with limit value 1 when fr approaches zero: */
  fder=2*cos(fr);
} else
  fder=2*cos(fr)*(sin(fr)/fr);

/* Objective function: */
if (calcobj)
{
  obj=ff;
  if (printlevel>=1)
    printf("\nResults:\nObjective function: %g\n",obj);
}
/* Constraint functions: */
if (calcconstr)
{
  constr[0]=TX+y*y-x;
  constr[1]=TY-y;
  if (printlevel>=1)
  {
    printf("Constraint f.: {%g, %g}\n",constr[0],constr[1]);
  }
}
/* Objective function gradient: */
if (calcgradobj)
{
  gradobj[0]=fder*A*(x-CX);
  gradobj[1]=fder*B*(y-CY);
  if (printlevel>=1)
  {
    printf("Grad. obj.: {%g, %g}\n",gradobj[0],gradobj[1]);
  }
}
/* Constraint function gradients: */
if (calcgradconstr)
{
  gradconstr[0]=-1;  /* d c1/d x */
  gradconstr[1]=2*y;  /* d c1/d y */
  gradconstr[2]=0;  /* d c2/d x */
  gradconstr[3]=-1;  /* d c2/d y */
  if (printlevel>=1)
  {
    printf("Grad. constr.: {{%g, %g}, {%g, %g}}\n",
      gradconstr[0],gradconstr[1],gradconstr[2],gradconstr[3]);
  }
}
if (printlevel>=1)
  printf("Return value: %i\n",ret);

/* OUTPUT DIRECT ANALYSIS RESULTS: */
retaux= writeanalysisoutput(outfilename, numparam, numconstr,
     param, obj, constr, gradobj, gradconstr, 
     calcobj, calcconstr, calcgradobj, calcgradconstr, ret );
if (ret==0)
  ret=retaux;
#undef MAXBUF
#undef NUMPAR
#undef NUMCONSTR
return ret;
}



int analysis1(char *infilename,char *outfilename)
    /* Analysis function that reads the input data (optimization parameters
    and calculation flags) from the file infile in standard IOptLib format, 
    calculates the response functions and their gradients, and writes the
    results to the output file file named outfilename.
      This analysis function represents the following two parameric nonlinear
    constraint optimization problem:
    min f(x,y)=x^2+y^4, subject to y>=(x-3)^6 and y>=17-x^2, 
    therefore f(x,y)=x^2+y^4, c_1(x,y)=(x-3)^6-y, c_2(x,y)=17-x^2-y.
    Solution: {4,1}, objective func. in optimum: 17.
    $A Igor sep06; */
{
#define MAXBUF 20   /* Buffer length */
#define NUMPAR 2   /* Number of parameters */
#define NUMCONSTR 2   /* Number of constraints */
int numparam=NUMPAR,
    numconstr=NUMCONSTR,
    numc=0,num=0,i=0,j=0,
    ret=0,retaux,
    calcobj=0,calcconstr=0,calcgradobj=0,calcgradconstr=0,
    reqcalcobj=0,reqcalcconstr=0,reqcalcgradobj=0,reqcalcgradconstr=0;
double x,y,obj=0,
       param[NUMPAR]={0},
       constr[NUMCONSTR]={0},
       gradobj[NUMPAR]={0},
       gradconstr[NUMCONSTR*NUMPAR]={0};
FILE *infile=NULL,*outfile=NULL;

obj=0; constr[0]=constr[1]=0;
gradobj[0]=gradobj[1]=0.;
gradconstr[0]=gradconstr[1]=gradconstr[2]=gradconstr[3]=0.;

if (printlevel>=1)
{
  printf("\nDirect analysis No. 1 (2 param., 2 constr.),\n  input file: \"%s\", output file: \"%s\"\n",
        infilename,outfilename);
}

/* Read input data (optimization parameters & calculation request flags): */
ret=readanalysisinput(infilename,param,numparam,&numparam,
    &reqcalcobj,&reqcalcconstr,&reqcalcgradobj,&reqcalcgradconstr);
calcobj=reqcalcobj;  calcconstr=reqcalcconstr;
calcgradobj=reqcalcgradobj;  calcgradconstr=reqcalcgradconstr;

/* CALCULATE RESPONSE FUNCTIONS: */
x=param[0];  y=param[1];
/* Objective function: */
if (calcobj)
{
  obj=x*x+y*y*y*y;
  if (printlevel>=1)
    printf("\nResults:\nObjective function: %g\n",obj);
}
/* Constraint functions: */
if (calcconstr)
{
  constr[0]=(x-3)*(x-3)*(x-3)*(x-3)*(x-3)*(x-3)-y;
  constr[1]=17-x*x-y;
  if (printlevel>=1)
  {
    printf("Constraint f.: {%g, %g}\n",constr[0],constr[1]);
  }
}
/* Gradient of the objective function: */
if (calcgradobj)
{
  gradobj[0]=2*x;
  gradobj[1]=4*y*y*y;
  if (printlevel>=1)
  {
    printf("Grad. obj.: {%g, %g}\n",gradobj[0],gradobj[1]);
  }
}
/* Gradients of the constraint functions: */
if (calcgradconstr)
{
  gradconstr[0]=6*(x-3)*(x-3)*(x-3)*(x-3)*(x-3);  /* d c1/d x */
  gradconstr[1]=-1;  /* d c1/d y */
  gradconstr[2]=-2*x;  /* d c2/d x */
  gradconstr[3]=-1;  /* d c2/d y */
  if (printlevel>=1)
  {
    printf("Grad. constr.: {{%g, %g}, {%g, %g}}\n",
      gradconstr[0],gradconstr[1],gradconstr[2],gradconstr[3]);
  }
}
if (printlevel>=1)
  printf("Return value: %i\n",ret);

/* OUTPUT DIRECT ANALYSIS RESULTS: */
retaux= writeanalysisoutput(outfilename, numparam, numconstr,
     param, obj, constr, gradobj, gradconstr, 
     calcobj, calcconstr, calcgradobj, calcgradconstr, ret );
if (ret==0)
  ret=retaux;

#undef MAXBUF
#undef NUMPAR
#undef NUMCONSTR
return ret;
}






int analysis2(char *infilename,char *outfilename)
    /* Analysis function with variable number of parameters.
      This analysis function represents the following two parameric nonlinear
    constraint optimization problem:
    min f(x,y)=x^2+y^4, subject to y>=(x-3)^6 and y>=17-x^2, 
    therefore f(x,y)=x^2+y^4, c_1(x,y)=(x-3)^6-y, c_2(x,y)=17-x^2-y.
    Solution: {4,1}, objective func. in optimum: 17.
    $A Igor sep06; */
{
#define NUMPAR 10   /* Maximal number of parameters */
#define NUMCONSTR NUMPAR   /* Maximal number of constraints */
#define MAXBUF NUMPAR+1   /* Buffer length */
int numparam=NUMPAR,
    numconstr=NUMCONSTR,
    numc=0,num=0,i=0,j=0,
    ret=0,retaux,
    calcobj=0,calcconstr=0,calcgradobj=0,calcgradconstr=0,
    reqcalcobj=0,reqcalcconstr=0,reqcalcgradobj=0,reqcalcgradconstr=0;
double obj=0,
       param[NUMPAR]={0},
       constr[NUMCONSTR]={0},
       gradobj[NUMPAR]={0},
       gradconstr[NUMCONSTR*NUMPAR]={0};
FILE *infile=NULL,*outfile=NULL;

obj=0; constr[0]=constr[1]=0;
gradobj[0]=gradobj[1]=0.;
gradconstr[0]=gradconstr[1]=gradconstr[2]=gradconstr[3]=0.;

if (printlevel>=1)
{
  printf("\nDirect analysis No. 2 (quad. obj., lin. constr., variable numconstr=numparam),\n  input file: \"%s\", output file: \"%s\"\n",
        infilename,outfilename);
}

/* Read input data (optimization parameters & calculation request flags): */
numparam=0; /* indicates that number of parameters is unknown (established
    after reading parameters from the input file) */
ret=readanalysisinput(infilename,param,NUMPAR,&numparam,
    &reqcalcobj,&reqcalcconstr,&reqcalcgradobj,&reqcalcgradconstr);
calcobj=reqcalcobj;  calcconstr=reqcalcconstr;
calcgradobj=reqcalcgradobj;  calcgradconstr=reqcalcgradconstr;
/* Number of constraints is in this case equal to number of parameters: */
numconstr=numparam;

/* CALCULATE RESPONSE FUNCTIONS: */
/* Objective function: */
if (calcobj)
{
  obj=0;
  for (i=0;i<numparam;++i)
    obj+=param[i]*param[i];
  if (printlevel>=1)
    printf("\nResults:\nObjective function: %g\n",obj);
}
/* Constraint functions: */
if (calcconstr)
{
  for (i=0;i<numconstr;++i)
    constr[i]=1-param[i];
  if (printlevel>=1)
  {
    printf("Constraint f.: {");
    for (i=0;i<numconstr;++i)
    {
      printf("%g",constr[i]);
      if (i<numconstr-1)
        printf(", ");
      else
        printf("}\n");
    }
  }
}
/* Gradient of the objective function: */
if (calcgradobj)
{
  for (i=0;i<numparam;i=i+1)
    gradobj[i]=2*param[i];
  if (printlevel>=1)
  {
    printf("Objective gradient: {");
    for (i=0;i<numparam;++i)
    {
      printf("%g",gradobj[i]);
      if (i<numparam-1)
        printf(", ");
      else
        printf("}\n");
    }
  }
}
/* Gradients of the constraint functions: */
if (calcgradconstr)
{
  for (i=0;i<numconstr;++i)
    for (j=0;j<numparam;++j)
      gradconstr[i*numparam+j]=i==j?-1:0;
  if (printlevel>=1)
  {
    printf("Grad. constr.: {");
    for (i=0;i<numconstr;++i)
    {
      printf("{");
      for (j=0;j<numparam;++j)
      {
        printf("%g",gradconstr[i*numparam+j]);
        if (j<numparam-1)
          printf(", ");
        else
          printf("}");
      }
      if (i<numconstr-1)
        printf(", ");
      else
        printf("}\n");
    }
  }
}
if (printlevel>=1)
  printf("Return value: %i\n",ret);

/* OUTPUT DIRECT ANALYSIS RESULTS: */
retaux= writeanalysisoutput(outfilename, numparam, numconstr,
     param, obj, constr, gradobj, gradconstr, 
     calcobj, calcconstr, calcgradobj, calcgradconstr, ret );
if (ret==0)
  ret=retaux;

#undef MAXBUF
#undef NUMPAR
#undef NUMCONSTR
return ret;
}




int main(int argc, char *argv[],char *env[])
{
int whichanalysis=0;
if (argc<3)
{
  printf("\n\nERROR: Too few command line arguments.\n");
  printf("Run the program as \n  %s infile outfile <whichanalysis <printlevel>> \n",
      argv[0]);
  printf("0: 2 parameters, 2 constraints,\n  the same as example from command file\n");
  printf("1: 2 parameters, 2 constraints.\n");  
  printf("3: variable num. par. = num. constr., quadratic objective, lin. constr.\n");
  printf("\n");
} else
{
  if (argc>3)
    whichanalysis=atoi(argv[3]);
  if (argc>4)
    printlevel=atoi(argv[4]);
  switch(whichanalysis)
  {
    /* Peform a specific direct analysis: */
    case 1:
      analysis1(argv[1],argv[2]);
      break;
    case 2:
      analysis2(argv[1],argv[2]);
      break;
    default:
      analysis(argv[1],argv[2]);
      if (whichanalysis!=0)
        printf("\n\nERROR: unknown analysis type %i, default will be used (0).\n",whichanalysis);
  }
}
}


